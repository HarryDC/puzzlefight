using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Spells;

[GlobalClass]
public abstract partial class Spell : Resource
{
    [Export] public int Level = 1;

    public string Name { get; protected set; }= "";
    public string Effect { get; protected set; } = "";
    
    public string Description { get; protected set; } = "";
    
    protected List<MatchData> Material = new();
    
    public bool CanCast(Character caster)
    {
        Debug.Assert(caster is not null);
        return caster.HasGems(Material);
    }
    
    public virtual bool Cast(Character caster)
    {
        Debug.Assert(caster is not null);
        if (!CanCast(caster)) return false;
        caster.RemoveGems(Material);
        return true;
    }
}