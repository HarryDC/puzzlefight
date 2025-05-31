using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public interface IParticipant
{
    public void Setup(BoardNode board);
    public void TakeTurn();
}