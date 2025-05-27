using Godot;

namespace PuzzleFight.Nodes;

public partial class AiPlayerNode : Node, IParticipant
{
    public void TakeTurn()
    {
        GD.Print("AI Taking Turn");
    }
}