using Godot;
using System;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class BoardNode : Node
{
    [Export] public int Width { get; set; } = 5;
    [Export] public int Height { get; set; } = 5;
    
    [Export] public float Spacing { get; set; } = 120f;
    
    private Common.Board _board;

    public override void _Ready()
    {
        _board = new Board(Width, Height);

        Vector2 origin = new Vector2();
        
        for (var x = 0; x < _board.Width; x++)
        {
            for (var y = 0; y < _board.Height; y++)
            {
                var type = _board.Data[x, y];
                var sprite = new Sprite2D();
                sprite.Texture = GD.Load<Texture2D>($"res://assets/gems/{type}.png");
                var pos = new Vector2(x, y) * Spacing;
                sprite.Position = origin + pos;
                AddChild(sprite);
            }
        }
    }
}
