using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Nodes;

public abstract partial class Participant : Node
{
    [Export] public Character Character;
    [Export] public Participant Opponent;
    [Export] public ScorePanel ScorePanel;
    
    [Signal]
    public delegate void ParticipantDeathEventHandler();
    
    public abstract void Setup(BoardNode board);
    public abstract void TakeTurn();
    
    public override void _Ready()
    {
        ScorePanel.UpdateCharacter(Character);
    }

    public void Attack(Participant attacker, int damage)
    {
        GD.Print($"{attacker} attacking {Name} for {damage} damage");
        var realDamage = Math.Max(0, damage - Character.Armor);
        Character.HitPoints -= realDamage;
        GD.Print($"{Name} hit for {realDamage} damage");
        
        ScorePanel.UpdateCharacter(Character);
        
        if (Character.HitPoints <= 0)
        {
            EmitSignal(SignalName.ParticipantDeath);
        }
    }
    
    public void DidMatch(List<MatchData> matches)
    {
        var attackCount =  (from match in matches 
            where match.Type == StoneTypeEnum.Sword select match).Sum(m => m.Count);
        
        var defenceCount = (from match in matches
            where match.Type == StoneTypeEnum.Shield select match).Sum(m => m.Count);
        
        // Accumulate here, reset in PreMove
        Character.TempArmor += defenceCount;
        Character.Armor += defenceCount;
        
        var attack = (3.0f / matches.Count) * Character.Attack;
        
        if (attackCount > 0)
        {
            Opponent.Attack(this, (int)attack);
        }
        ScorePanel.UpdateScores(matches);
        ScorePanel.UpdateCharacter(Character);
    }
}