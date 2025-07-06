using System;
using System.Diagnostics;
using Godot;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Participant
{
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                BoardNode.ClearSelection();
                return;
            }

            if (mouseEvent.ButtonIndex != MouseButton.Left) return;
            
            // Convert mouse position to grid position
            BoardNode.SelectPiece(mouseEvent.Position);
        }            
    }
    
    public override void Setup(BoardNode board)
    {
        BoardNode = board;
        Actions = Character.Actions;
    }

    public override void TakeTurn()
    {
        Actions -= 1;
        Character.PreMoveUpdate();
    }

    public override void EndRound()
    {
        Actions = Character.Actions;
    }
}
