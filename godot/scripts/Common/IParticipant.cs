using System.Collections.Generic;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public interface IParticipant
{
    public void Setup(BoardNode board);
    public void TakeTurn();
    public void DidMatch(List<MatchData> matches);
}