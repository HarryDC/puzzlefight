using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Node, IParticipant
{
    [Export] private Panel _scorePanel;
    
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
        if (_scorePanel is ScorePanel sp)
        {
            sp.UpdateScores(matches);
        }
        else
        {
            GD.Print("Cast failed!");
        }
    }
}