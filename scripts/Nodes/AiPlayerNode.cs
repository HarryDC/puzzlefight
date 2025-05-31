using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class AiPlayerNode : Node, IParticipant
{
    private BoardNode _boardNode;

    public void Setup(BoardNode boardNode)
    {
        _boardNode = boardNode;
        
    }

    public void TakeTurn()
    {
        var moves = _boardNode.Board.GetAllMoves();
        // Should not be empty
        var count = moves.Count/2;
        var chosen = (int)(GD.Randi() % count);
        _boardNode.StartSwap(moves[chosen*2], moves[chosen*2+1]);
    }
}