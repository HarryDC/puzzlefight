using Godot;
namespace PuzzleFight.Resources.Equipment;

public enum Type
{
    Armor,
    Weapon,
}

public enum Slot
{
    Hand,
    TwoHand,
    Head,
    Torso,
    Finger,
}

[GlobalClass]
public abstract partial class Equipment : Resource
{
    [Export]
    public Type Type { get; protected set; }
    
    [Export]
    public Slot Slot { get; protected set; }
    
    [Export]
    public Texture2D Image { get; protected set; }
}

public partial class Armor : Equipment
{
    [Export] public int Defense;
}