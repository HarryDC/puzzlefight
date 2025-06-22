namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Participant
{
    // TODO move piece selection for move into here
    BoardNode _boardNode;

    public override void Setup(BoardNode board)
    {
        _boardNode = board;
    }

    public override void TakeTurn()
    {
        Character.PreMoveUpdate();
    }
}
