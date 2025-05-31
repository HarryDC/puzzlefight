using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Node, IParticipant
{
    BoardNode _boardNode;
    public void Setup(BoardNode board)
    {
        _boardNode = board;
    }

    public void TakeTurn()
    {
        GD.Print("Human Taking Turn");
    }
}