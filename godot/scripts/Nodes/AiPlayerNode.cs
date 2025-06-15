using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class AiPlayerNode : Participant
{
    private BoardNode _boardNode;
    
    [Export] public Participant Opponent;
    
    [Export] private Node2D _selectionSprite1;
    [Export] private Node2D _selectionSprite2;
   
    public override void Setup(BoardNode boardNode)
    {
        _boardNode = boardNode;
    }

    public override void TakeTurn()
    {
        var moves = _boardNode.Board.GetAllMoves();
        // Should not be empty
        var count = moves.Count/2;
        var chosen = (int)(GD.Randi() % count);
        
        _boardNode.StartAiMove(moves[chosen*2 ], moves[chosen*2+1]);

    }
    
    public override void DidMatch(List<MatchData> matches)
    {
        var attackCount =  (from match in matches 
                where match.Type == StoneTypeEnum.Sword select match).Sum(m => m.Count);
        
        var attack = (3.0f / matches.Count) * Character.Attack;
        
        
        if (attackCount > 0)
        {
            Opponent.Attack(this, (int)attack);
        }
        
        ScorePanel.UpdateScores(matches);
    }
}