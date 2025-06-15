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
    public abstract void Setup(BoardNode board);
    public abstract void TakeTurn();
    public abstract void DidMatch(List<MatchData> matches);

    public void Attack(Participant attacker, int damage)
    {
        GD.Print($"{attacker} attacking {Name} for {damage} damage");
        var realDamage = Math.Max(0, damage - Character.Armor);
        Character.HitPoints -= realDamage;
        GD.Print($"{Name} hit for {realDamage} damage");
        
        ScorePanel.UpdateCharacter(Character);
        
        if (Character.HitPoints <= 0)
        {
            GD.Print("Character Ded");
        }
    }
}