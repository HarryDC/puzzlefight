using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class AddAttack : Spell
{
    [Export] public int Amount = 2;
    
    public AddAttack()
    {
        Name = Tr("Add Attack");
        Effect = $"+{Amount}Atk";
        Description = $"Level {Level} {Tr("Add Attack")} {Amount}";
        Material = new List<MatchData> { new MatchData(5, StoneTypeEnum.GemGreen) };
    }

    public override bool Cast(Character caster)
    {
        if (!base.Cast(caster)) return false;
        caster.Attack += Amount;
        return true;
    }
}