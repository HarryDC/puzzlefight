using Godot;
using System;
using System.Collections.Generic;
using PuzzleFight.Common;
using PuzzleFight.scripts.Resources;

public partial class ScorePanel : Panel
{
    [Export] public Label RedLabel;
    [Export] public Label GreenLabel;
    [Export] public Label BlueLabel;
    [Export] public Label HpLabel;
    [Export] public Label AcLabel;

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
        GD.Print("ScorePanel Initialized");
    }

    public void UpdateScores(List<MatchData> matches)
    {
        foreach (var match in matches)
        {
            if (_scores.ContainsKey(match.Type))
            {
                _scores[match.Type].Score += match.Count;
            }
            else
            {
                GD.PrintErr($"ScorePanel Can't add {match.Type} to score list");
            }
        }
    }

    public void UpdateCharacter(Character character)
    {
        HpLabel.Text = $"{character.HitPoints}";
        AcLabel.Text = $"{character.Armor}";
    }

    public void Clear()
    {
        foreach (var stoneType in _scores.Keys)
        {
            _scores[stoneType].Score = 0;
        }
    }
}
