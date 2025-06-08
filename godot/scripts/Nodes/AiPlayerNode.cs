using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class AiPlayerNode : Node, IParticipant
{
    private BoardNode _boardNode;
    [Export] ScorePanel _scorePanel;

    [Export] private Node2D _selectionSprite1;
    [Export] private Node2D _selectionSprite2;
    
    // TODO need to animate the move selection it's hard to tell what's going on

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
        
        _boardNode.StartAiMove(moves[chosen*2 ], moves[chosen*2+1]);

    }
    
    public void DidMatch(List<MatchData> matches)
    {
        if (_scorePanel is ScorePanel p)
        {
            p.UpdateScores(matches);
        }
    }
}