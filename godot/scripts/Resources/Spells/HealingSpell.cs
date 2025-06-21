using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class HealingSpell : Spell
{
    [Export] public int Amount = 5;
    
    public HealingSpell()
    {
        Name = "Healing";
        Material = new() { new MatchData(1, StoneTypeEnum.GemRed) };
    }

    public override bool Cast()
    {
        if (!base.Cast()) return false;
        Caster.HitPoints += Amount;
        return true;
    }
}