using Godot;
using System;
using System.Collections.Generic;
using PuzzleFight.Common;

public partial class ScorePanel : Panel
{
    [Export] public Label RedLabel;
    [Export] public Label GreenLabel;
    [Export] public Label BlueLabel;
    [Export] public Label PurpleLabel;
    [Export] public Label YellowLabel;

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
        _scores.Add(StoneTypeEnum.GemPurple, new ScoreData(PurpleLabel));
        _scores.Add(StoneTypeEnum.GemYellow, new ScoreData(YellowLabel));
        GD.Print("ScorePanel Initialized");
    }

    public void UpdateScores(List<MatchData> matches)
    {
        foreach (var match in matches)
        {
            _scores[match.Type].Score += match.Count;
        }
    }

    public void Clear()
    {
        foreach (var stoneType in _scores.Keys)
        {
            _scores[stoneType].Score = 0;
        }
    }
}
