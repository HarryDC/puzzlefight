using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class AddAttack : Spell
{
    [Export] public int Amount = 2;
    
    public AddAttack()
    {
        Name = Tr("Add Attack");
        Material = new List<MatchData> { new MatchData(5, StoneTypeEnum.GemGreen) };
    }

    public override bool Cast()
    {
        if (!base.Cast()) return false;
        Caster.Attack += Amount;
        return true;
    }
}