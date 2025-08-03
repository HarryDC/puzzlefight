using Ink.Runtime;
using Godot;

namespace PuzzleFight.Nodes;

public partial class GameManager : Node
{
    public Story Story;
    public string Encounter;

    public void LoadEncounter(string encounter)
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }

    public void LoadKnot(string Knot)
    {
        
    }

    public void SwitchToStory(string result)
    {
        GetTree().ChangeSceneToFile("res://scenes/dialog_fullscreen.tscn");
    }
}