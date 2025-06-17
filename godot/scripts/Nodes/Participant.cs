using System;
using System.Collections.Generic;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

namespace PuzzleFight.Nodes;

public abstract partial class Participant : Node
{
    [Export] public Character Character;
    [Export] public ScorePanel ScorePanel;
    
    [Signal]
    public delegate void ParticipantDeathEventHandler();
    
    public abstract void Setup(BoardNode board);
    public abstract void TakeTurn();
    public abstract void DidMatch(List<MatchData> matches);

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
}