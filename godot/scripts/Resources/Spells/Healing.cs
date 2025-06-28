using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class Healing : Spell
{
    [Export] public int Amount = 5;
    
    public Healing()
    {
        Name = Tr("Healing");
        Effect = $"+{Amount}HP";
        Description = $"Level {Level} {Tr("Healing")} {Amount}HP";
        Material = new List<MatchData> { new MatchData(5, StoneTypeEnum.GemRed) };
    }

    public override bool Cast(Character caster)
    {
        if (!base.Cast(caster)) return false;
        caster.HitPoints += Amount;
        return true;
    }
}