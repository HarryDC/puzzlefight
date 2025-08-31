using System;
using System.Diagnostics;
using Godot;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Participant
{
    private bool _isMyTurn = false;
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (BoardNode.IsAnimating || !_isMyTurn) return;
            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                BoardNode.ClearSelection();
                return;
            }

            if (mouseEvent.ButtonIndex != MouseButton.Left) return;
            
            // If true is returned a valid move was made 
            if (BoardNode.SelectPiece(mouseEvent.Position))
            {
                Actions -= 1;
            }
        }            
    }
    
    public override void Setup(BoardNode board)
    {
        BoardNode = board;
        Character = GameManager.Instance.PlayerCharacter;
        if (Character == null) GD.PrintErr("Character is NULL in HumanPlayerNode");
        Actions = Character.Actions;
    }

    public override void BeginRound()
    {
        _isMyTurn = true;
        Actions = Character.Actions;
        ScorePanel.EnableButtons();
    }

    public override void TakeTurn()
    {
        Character.PreMoveUpdate();
    }

    public override void EndRound()
    {
        _isMyTurn = false;
        ScorePanel.BlockButtons();
    }
}
