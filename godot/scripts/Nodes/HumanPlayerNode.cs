using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Node, IParticipant
{
    // TODO move piece selection for move into here
    [Export] public Panel ScorePanel;
    
    BoardNode _boardNode;
    
    public void Setup(BoardNode board)
    {
        _boardNode = board;
    }

    public void TakeTurn()
    {
        GD.Print("Human Taking Turn");
    }

    public void DidMatch(List<MatchData> matches)
    {
        if (ScorePanel is ScorePanel sp)
        {
            sp.UpdateScores(matches);
        }
        else
        {
            GD.Print("Cast failed!");
        }
    }
}