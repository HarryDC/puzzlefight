using Godot;
using System;

public partial class MainMenu : Node2D
{
    void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }
}
