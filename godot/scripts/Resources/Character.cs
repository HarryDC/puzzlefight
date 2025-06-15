using Godot;

namespace PuzzleFight.scripts.Resources;

[GlobalClass]
public partial class Character : Resource
{
    [Export] public int HitPoints { get; set; }
    [Export] public int MaxHitPoints { get; set; }
    [Export] public int Armor { get; set; }
    [Export] public int Attack { get; set; }

    public Character()
    {
        HitPoints = 0;
        MaxHitPoints = 0;
        Armor = 0;
        Attack = 0;
    }

}