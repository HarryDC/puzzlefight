using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class Healing : Spell
{
    [Export] public int Amount = 5;
    
    public Healing()
    {
        Name = Tr("Healing");
        Material = new List<MatchData> { new MatchData(5, StoneTypeEnum.GemRed) };
    }

    public override bool Cast()
    {
        if (!base.Cast()) return false;
        Caster.HitPoints += Amount;
        return true;
    }
}