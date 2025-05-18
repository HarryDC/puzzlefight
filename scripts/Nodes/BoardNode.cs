using Godot;
using System;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class BoardNode : Node2D
{
    [Export] public int Width { get; set; } = 5;
    [Export] public int Height { get; set; } = 5;
    
    [Export] public float Spacing { get; set; } = 120f;
    
    [Export] Sprite2D SelectionSprite { get; set; }

    
    private Common.Board _board;
    private Array2D<Sprite2D> _sprites;
    
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseEvent)
        {
            // Convert mouse position to grid position
            var local = mouseEvent.Position - Position + new Vector2(Spacing * .5f, Spacing * .5f);
            GD.Print("mouse button event at ", local);
            if (local.X < 0 || local.Y < 0 || local.X > Width * Spacing || local.Y > Height * Spacing)
            {
                return;
            }
            var x = (int)Math.Floor(local.X / Spacing);
            var y = (int)Math.Floor(local.Y / Spacing);
            GD.Print("mouse button event at {0} {1}", x, y);
            // Swap pieces
            
            SelectionSprite.Position = new Vector2(x, y) * Spacing;
            
        }
    }

    public override void _Ready()
    {
        _board = new Board(Width, Height);
        _sprites = new Array2D<Sprite2D>(Width, Height);
        
        for (var x = 0; x < _board.Width; x++)
        {
            for (var y = 0; y < _board.Height; y++)
            {
                var type = _board.Data[x, y];
                var sprite = new Sprite2D();
                sprite.Texture = GD.Load<Texture2D>($"res://assets/gems/{type}.png");
                var pos = new Vector2(x, y) * Spacing;
                sprite.Position = pos;
                _sprites[x,y] = sprite;
                AddChild(sprite);
            }
        }
    }
}
