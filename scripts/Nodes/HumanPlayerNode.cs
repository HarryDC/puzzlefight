using Godot;

namespace PuzzleFight.Nodes;

public partial class HumanPlayerNode : Node, IParticipant
{
    public void TakeTurn()
    {
        GD.Print("Human Taking Turn");
    }
}