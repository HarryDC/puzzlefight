using Godot;

namespace PuzzleFight.Resources.Equipment;

[GlobalClass]
public partial class Weapon : Equipment
{
    [Export]
    public int Damage { get; protected set; }
}