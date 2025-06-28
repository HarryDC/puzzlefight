using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class AddArmor : Spell
{
    [Export] public int Amount = 2;
    
    public AddArmor()
    {
        Name = Tr("Add Armor");
        Effect = $"+{Amount}AC";
        Description = $"Level {Level} {Tr("Add Armor")} {Amount}";
        Material = new List<MatchData> { new MatchData(5, StoneTypeEnum.GemBlue) };
    }

    public override bool Cast(Character caster)
    {
        if (!base.Cast(caster)) return false;
        caster.Armor += Amount;
        return true;
    }
}