using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.Nodes;
using PuzzleFight.Resources.Equipment;
using PuzzleFight.Spells;

namespace PuzzleFight.scripts.Resources;

[GlobalClass]
// This class will handle all the information on any Character
// The "current" values of all character properties will be stored 
// and avaialable here
public partial class Character : Resource
{
    /// <summary>
    /// The slots that a character -could- have
    /// maybe we will need to give each character actual slots that
    /// are a subset of these
    /// </summary>
    public enum Slot
    {
        LeftHand,
        RightHand,
    }
    [Export] public int HitPoints { get; set; }
    [Export] public int MaxHitPoints { get; set; }
    [Export] public int Armor { get; set; }
    [Export] public int Attack { get; set; }
    [Export] public int Actions { get; set; } = 1;
    [Export] public Godot.Collections.Array<Spell> Spells { get; set; } = new();
    [Export] public Godot.Collections.Array<Equipment> Equipment { get; set; } = new();
    [Export] public Godot.Collections.Dictionary<Slot, Equipment> Equipped { get; set; } = new();
    
    public int TempArmor {get; set;}
    public Dictionary<StoneTypeEnum, int> Stash { get; private set; } = new();
    
    private readonly StoneTypeEnum[] _gemTypes = { StoneTypeEnum.GemRed, StoneTypeEnum.GemBlue, StoneTypeEnum.GemGreen };

    private WeakRef _participant;

    public Participant Participant
    {
        get => _participant.GetRef().As<Participant>();
        set => _participant = WeakRef(value);
    }
    
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

    public bool Cast(Spell spell)
    {
        return spell.Cast(this);
    }


    public struct AttackResult
    {
        public int TotalAttack;
        public int TotalDefense;
        public int DamageDealt;
    }
    
    public AttackResult ExecuteAttack(Character other, int matchCount)
    {
        // Determine Total Damage
        int attack = Attack;
        foreach (var (slot, thing) in Equipped)
        {
            if (thing is Weapon weapon)
            {
                attack += weapon.Damage;
            }
        }
        
        // Determine Defense
        int defense = other.Armor;
        foreach (var (slot, thing) in other.Equipped)
        {
            if (thing is Armor armor)
            {
                defense += armor.Defense;
            }
        }

        AttackResult result;
        result.TotalAttack = attack;
        result.TotalDefense = defense;
        result.DamageDealt = Math.Max(0, attack - defense);

        other.HitPoints = Math.Max(0, other.HitPoints - result.DamageDealt);

        return result;
    }
    
}