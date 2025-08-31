#nullable enable
using System;
using Ink.Runtime;
using Godot;
using GodotInk;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Nodes;

[GlobalClass]
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public InkStory Story { get; private set; }
    public Guid GameGuid { get; private set; }
    public string EncounterText { get; set; } = "";

    public Character PlayerCharacter { get; private set; }
    public Character Opponent { get; private set; }

    public string LastResult = "";
    private string? _currentEncounterId;

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

    private void Gain(string id)
    {
        GD.Print("Character gains " + id);
    }

    public void DoEncounter()
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }

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

    public void LoadKnot(string knot)
    {
        
    }

    public void Victory()
    {
        SwitchToStory("Victory");
    }

    public void Defeat()
    {
        SwitchToStory("Defeat");
    }

    public void SwitchToStory(string result)
    {
        LastResult = result;
        _currentEncounterId = null;
        GetTree().ChangeSceneToFile("res://scenes/dialog_fullscreen.tscn");
    }

    public bool HasEncounter()
    {
        return _currentEncounterId != null;
    }
}