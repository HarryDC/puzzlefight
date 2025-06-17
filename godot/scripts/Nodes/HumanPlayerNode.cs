using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Participant
{
    // TODO move piece selection for move into here
    [Export] public Participant Opponent;
    
    BoardNode _boardNode;
    
    public override void Setup(BoardNode board)
    {
        _boardNode = board;
    }

    public override void TakeTurn()
    {
        GD.Print("Human Taking Turn");
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