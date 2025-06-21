using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.Spells;

namespace PuzzleFight.scripts.Resources;

[GlobalClass]
// This class will handle all the information on any Character
// The "current" values of all character properties will be stored 
// and avaialable here
public partial class Character : Resource
{
    [Export] public int HitPoints { get; set; }
    [Export] public int MaxHitPoints { get; set; }
    [Export] public int Armor { get; set; }
    [Export] public int Attack { get; set; }
    
    [Export]
    public Godot.Collections.Array<Spell> Spells { get; set; } = new Godot.Collections.Array<Spell>();
    
    public int TempArmor {get; set;}
    public Dictionary<StoneTypeEnum, int> Stash { get; private set; } = new();
    
    private readonly StoneTypeEnum[] _gemTypes = { StoneTypeEnum.GemRed, StoneTypeEnum.GemBlue, StoneTypeEnum.GemGreen };
    
    public Character()
    {
        HitPoints = 0;
        MaxHitPoints = 0;
        Armor = 0;
        Attack = 0;
        foreach (var type in _gemTypes)
        {
            Stash[type] = 0;
        }

        foreach (var spell in Spells)
        {
            spell.Caster = this;
        }
    }

    public void PreMoveUpdate()
    {
        Armor -= TempArmor;
        TempArmor = 0;
    }

    public void PostMoveUpdate()
    {
        Armor += TempArmor;
    }

    public void AddGems(List<MatchData> gems)
    {
        foreach (var gem in gems)
        {
            if (_gemTypes.Contains(gem.Type))
            {
                Stash[gem.Type]+=gem.Count;
            }
        }
    }

    public void RemoveGems(List<MatchData> gems)
    {
        if (!HasGems(gems)) return;
        foreach (var gem in gems)
        {
            Stash[gem.Type] -= gem.Count;
        }
    }

    public bool HasGems(List<MatchData> material)
    {
        foreach (var gem in material)
        {
            if (!_gemTypes.Contains(gem.Type) || Stash[gem.Type] < gem.Count)
                return false;
        }
        return true;
    }
}