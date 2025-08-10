using Godot;
using System;
using PuzzleFight.Nodes;

public partial class Game : Node
{
    void OnPlayerDeath()
    {
        GameManager.Instance.Defeat();
    }

    void OnOpponentDeath()
    {
        GameManager.Instance.Victory();
    }
}
