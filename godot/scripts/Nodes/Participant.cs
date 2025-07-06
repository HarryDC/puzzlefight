using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;
using PuzzleFight.Spells;

namespace PuzzleFight.Nodes;

/// <summary>
/// Node type for an entity in the game (the player, any opponent). Each entity can trigger actions
/// dole out damage, etc
/// </summary>
/// <remarks>
/// Each Participant (Node) has a Character (RPG Attributes) attached to it, try and use the Participant
/// when interacting between Godot Nodes, and the Character when interacting on the Game System level
/// Eg. An attack will trigger the actual cause of damage on the Character class, but the UI effect
/// will be triggered on the Participant level
/// </remarks>
public abstract partial class Participant : Node
{
    [Export] public Character Character;
    [Export] public Participant Opponent;
    [Export] public ScorePanel ScorePanel;
    [Export] public TextHandler.Position Position;
    
    [Signal] public delegate void ParticipantDeathEventHandler();
    
    [Signal] public delegate void TextDisplayEventHandler(string text, int position);

    public int Actions = 0;

    protected BoardNode BoardNode;
    
    public abstract void Setup(BoardNode board);
    
    
    public abstract void BeginRound();
    
    /// Called whenever this participant is supposed to make a move
    public abstract void TakeTurn();
    /// <summary>
    /// Called whenever the participant is finished with the round
    /// </summary>
    public abstract void EndRound();
    
    public override void _Ready()
    {
        Character.Participant = this;
        
        ScorePanel.UpdateCharacter(Character);

        var textHandler = GetNode<TextHandler>("/root/Game/TextHandler");
        TextDisplay += textHandler.OnEmitText;
    }

    public void Attack(Participant attacker, int damage)
    {
        var realDamage = Math.Max(0, damage - Character.Armor);
        Character.HitPoints -= realDamage;
        
        EmitSignal(SignalName.TextDisplay, $"-{realDamage}HP", (int)Position);
        
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
        
        if (defenceCount > 0)
        {
            // Accumulate here, reset in PreMove
            Character.TempArmor += defenceCount;
            Character.Armor += defenceCount;
        
            EmitSignal(SignalName.TextDisplay, $"+{defenceCount}AC", (int)Position);
        }
        
        
        var hasFive = (from match in matches select match).Any(m => m.Count == 5);
        if (hasFive)
        {
            ++Actions;
            EmitSignalTextDisplay("Bonus Action", (int)Position);
        }
        
        Character.AddGems(matches);
        
        var attack = (3.0f / matches.Count) * Character.Attack;
        
        if (attackCount > 0)
        {
            Opponent.Attack(this, (int)attack);
        }
    }

    public void Cast(Spell spell)
    {
        if (Actions < 1) return;
        --Actions;
        spell.Cast(Character);
        EmitSignal(SignalName.TextDisplay, spell.Effect, (int)Position);
        BoardNode.CheckEndRound();
    }

}