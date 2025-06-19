using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;

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
    
    public int TempArmor {get; set;}
    private Dictionary<StoneTypeEnum, int> _stash = new();
    
    
    public Character()
    {
        HitPoints = 0;
        MaxHitPoints = 0;
        Armor = 0;
        Attack = 0;
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

}