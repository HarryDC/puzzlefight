namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Participant
{
    // TODO move piece selection for move into here
    public override void Setup(BoardNode board)
    {
        BoardNode = board;
        Actions = Character.Actions;
    }

    public override void TakeTurn()
    {
        Actions -= 1;
        Character.PreMoveUpdate();
    }

    public override void EndRound()
    {
        Actions = Character.Actions;
    }
}
