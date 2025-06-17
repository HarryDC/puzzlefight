using Godot;
using System;

public partial class Game : Node
{
    void OnPlayerDeath()
    {
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }

    void OnOpponentDeath()
    {
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }
}
