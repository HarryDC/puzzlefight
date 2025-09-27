#nullable enable
using System;
using Ink.Runtime;
using Godot;
using GodotInk;
using PuzzleFight.Resources.Equipment;
using PuzzleFight.scripts.Resources;
using Type = PuzzleFight.Resources.Equipment.Type;

namespace PuzzleFight.Nodes;

/// <summary>
/// Game Manager, persistant class. Controls communication between inkle and the rest of the game
/// Controls the flow between the storyline and the match 3 sections
/// </summary>
[GlobalClass]
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public InkStory Story { get; private set; }
    /// <summary>
    /// UID for currently running game (use to differentiate loads)
    /// </summary>
    public Guid GameGuid { get; private set; }
    public string EncounterText { get; set; } = "";

    public Character PlayerCharacter { get; private set; }
    public Character Opponent { get; private set; }

    /// <summary>
    /// Whenever the game switches from match3 to story, use this as the choice
    /// that needs to be applied
    /// </summary>
    public string NextChoice = "";
    
    private string? _currentEncounterId;

    private static readonly string DefeatChoice = "Defeat";
    private static readonly string VictoryChoice = "Victory";

    public override void _Ready()
    {
        PlayerCharacter = ResourceLoader.Load<Character>("res://resources/player.tres");
        base._Ready();
        Instance = this;
        LoadStory("res://assets/ink/TheStandishHouse.ink");

        GameGuid = Guid.NewGuid();
    }
    
    private void LoadStory(string path)
    {
        try
        {
            Story = ResourceLoader.Load<InkStory>(path, null, ResourceLoader.CacheMode.Ignore);
            Story.BindExternalFunction("Encounter", Callable.From<string>(LoadEncounter), true);
            Story.BindExternalFunction("Gain", Callable.From<string>(Gain), true);
        }
        catch (InvalidCastException)
        {
            GD.PrintErr($"{path} is not a valid ink story. "
                        + "Please make sure it was imported with `is_main_file` set to `true`.");
        }
    }

    /// <summary>
    /// Callback from inkle to add an item to the players inventoy
    /// </summary>
    /// <param name="id">Id of the item (used to load item)</param>
    private void Gain(string id)
    {
        var filename = "res://resources/equipment/" + id + ".tres";
        var thing = ResourceLoader.Load<Equipment>(filename);
        if (thing == null)
        {
            GD.PrintErr("Could not load equipment from file:" + filename);
        }
        else
        {
            // TODO should probably ask the player if they want to equip the new item
            PlayerCharacter.Equipment.Add(thing);
            GD.Print("Player gained " + id);
            if (thing.Type == Type.Weapon)
            {
                PlayerCharacter.Equipped[Character.Slot.RightHand] = thing;
            }
        }
    }
    
    /// <summary>
    /// Callback from inkle to set the enemy that will be used the _next_ time
    /// this will usually be followed by a call to `DoEncounter`
    /// </summary>
    /// <param name="encounter"></param>
    private void LoadEncounter(string encounter)
    {
        _currentEncounterId = encounter;
        var filename = "res://resources/opponents/" + _currentEncounterId + ".tres";
        Opponent = ResourceLoader.Load<Character>(filename);
        if (Opponent == null)
        {
            GD.PrintErr("Could not load oppnent file " + filename);
        }
    }



    /// <summary>
    /// Indicate the player as won the current encounter
    /// </summary>
    public void Victory()
    {
        SwitchToStory(VictoryChoice);
    }

    /// <summary>
    /// Indicate the player has lost the current encounter
    /// </summary>
    public void Defeat()
    {
        SwitchToStory(DefeatChoice);
    }

    /// <summary>
    /// Change the scene to the Story
    /// </summary>
    /// <param name="nextChoice">Choice to be activated after the story is fully loaded</param>
    public void SwitchToStory(string nextChoice)
    {
        NextChoice = nextChoice;
        _currentEncounterId = null;
        GetTree().ChangeSceneToFile("res://scenes/dialog_fullscreen.tscn");
    }
    
    /// <summary>
    /// Loads the encounter screen (assumes that an enemy has be loaded)
    /// </summary>
    public void SwitchToEncounter()
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }

    public bool HasEncounter()
    {
        return _currentEncounterId != null;
    }
}