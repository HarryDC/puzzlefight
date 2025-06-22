using Godot;
using System.Collections.Generic;
using PuzzleFight.Common;
using PuzzleFight.Nodes;
using PuzzleFight.scripts.Resources;
using PuzzleFight.Spells;

public partial class ScorePanel : PanelContainer
{
    
    [Export] public Label RedLabel;
    [Export] public Label GreenLabel;
    [Export] public Label BlueLabel;
    [Export] public Label HpLabel;
    [Export] public Label AcLabel;
    [Export] public Label AtkLabel;

    public Character Character;
    
    private class ScoreData
    {
        public Label Label;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                Label.Text = $"{_score}";
            }
        }

        public ScoreData(Label label)
        {
            Label = label;
            Score = 0;
        }
    }
    
    private Dictionary<StoneTypeEnum, ScoreData> _scores = new();


    public override void _Ready()
    {
        _scores.Clear();
        _scores.Add(StoneTypeEnum.GemRed, new ScoreData(RedLabel));
        _scores.Add(StoneTypeEnum.GemBlue, new ScoreData(BlueLabel));
        _scores.Add(StoneTypeEnum.GemGreen, new ScoreData(GreenLabel));
    }
    
    public void UpdateScores(Character character)
    {
        foreach (var (type, count) in Character.Stash)
        {
            _scores[type].Score = count;
        }
    }

    public override void _Process(double delta)
    {
        UpdateCharacter(Character);
        UpdateScores(Character);
    }

    public void UpdateCharacter(Character character)
    {
        HpLabel.Text = $"{character.HitPoints}";
        AcLabel.Text = $"{character.Armor}";
        AtkLabel.Text = $"{character.Attack}";
        if (Character == null)
        {
            Character = character;
            foreach (var spell in Character.Spells)
            {
                AddSpell(spell);
            }
        }
    }

    public void Clear()
    {
        foreach (var stoneType in _scores.Keys)
        {
            _scores[stoneType].Score = 0;
        }
    }

    private void AddSpell(Spell spell)
    {
        var spellDisplay = ResourceLoader.Load<PackedScene>("res://scenes/spell_display.tscn").Instantiate<SpellDisplay>();
        var layout = GetNode<VBoxContainer>("VBoxContainer");
        spell.Caster = Character;
        spellDisplay.Spell = spell;
        layout.AddChild(spellDisplay);
    }
}
