using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Spells;

[GlobalClass]
public partial class Spell : Resource
{
    [Export] public int Level = 1;

    public string Name { get; protected set; }= "";

    [Export] public Character Caster;
    
    protected List<MatchData> Material = new();
    
    public bool CanCast()
    {
        Debug.Assert(Caster is not null);
        return Caster.HasGems(Material);
    }
    
    public virtual bool Cast()
    {
        Debug.Assert(Caster is not null);
        if (!CanCast()) return false;
        Caster.RemoveGems(Material);
        return true;
    }
}