using System;
using Godot;

namespace PuzzleFight.Common
{
    public enum StoneTypeEnum
    {
        None = -1,
        GemBlue,
        GemGreen,
        GemRed,
        GemPurple,
        GemYellow
    }

    public static class StoneType
    {
        private static readonly Array Values = Enum.GetValues(typeof(StoneTypeEnum));
        public static StoneTypeEnum Random()
        {
            if (Values.Length == 1) return (StoneTypeEnum)Values.GetValue(0);
            var stone = (StoneTypeEnum)Values.GetValue(GD.Randi() % (Values.Length - 1));
            //GD.Print($"New Stone {stone}");
            return stone;
        } 
    }
}