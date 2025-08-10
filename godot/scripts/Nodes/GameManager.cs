using System;
using Ink.Runtime;
using Godot;
using GodotInk;

namespace PuzzleFight.Nodes;

[GlobalClass]
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public InkStory Story { get; private set; }
    public string Encounter { get; private set; }
    public Guid GameGuid { get; private set; }
    
    public string LastResult = "";

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        Story = LoadStory("res://assets/ink/TheStandishHouse.ink");

        GameGuid = Guid.NewGuid();
    }
    
    private InkStory LoadStory(string path)
    {
        InkStory story = null;
        try
        {
            story = ResourceLoader.Load<InkStory>(path, null, ResourceLoader.CacheMode.Ignore);
        }
        catch (InvalidCastException)
        {
            GD.PrintErr($"{path} is not a valid ink story. "
                        + "Please make sure it was imported with `is_main_file` set to `true`.");
        }

        return story;
    }


    public void LoadEncounter(string encounter)
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }

    public void LoadKnot(string Knot)
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
        GetTree().ChangeSceneToFile("res://scenes/dialog_fullscreen.tscn");
    }
}