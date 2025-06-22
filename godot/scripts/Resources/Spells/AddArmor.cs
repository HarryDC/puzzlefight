using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class AddArmor : Spell
{
    [Export] public int Amount = 2;
    
    public AddArmor()
    {
        Name = Tr("Add Armor");
        Material = new List<MatchData> { new MatchData(5, StoneTypeEnum.GemBlue) };
    }

    public override bool Cast()
    {
        if (!base.Cast()) return false;
        Caster.Armor += Amount;
        return true;
    }
}