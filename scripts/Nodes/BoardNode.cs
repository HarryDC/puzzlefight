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

    
    private Vector2I? _selected;
    
    private Common.Board _board;
    private Array2D<Sprite2D> _sprites;
    
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                _selected = null;
                SelectionSprite.Visible = false;
                return;
            }

            if (mouseEvent.ButtonIndex != MouseButton.Left) return;
            
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
            if (_selected == null)
            {
                _selected = new Vector2I(x, y);
                SelectionSprite.Position = new Vector2(x, y) * Spacing;
                SelectionSprite.Visible = true;
            }
            else
            { 
                var target = new Vector2I(x, y);
                _board.Swap(_selected.Value, target);
                var sprite1 = _sprites[_selected.Value];
                var sprite2 = _sprites[target];
                sprite1.Position = IndexToPosition(target);
                sprite2.Position = IndexToPosition(_selected.Value);
                _sprites.Swap(_selected.Value, target); ;
                _selected = null;
                SelectionSprite.Visible = false;
            }            
        }
    }

    public override void _Ready()
    {
        _board = new Board(Width, Height);
        _sprites = new Array2D<Sprite2D>(Width, Height);
     
        _selected = null;
        SelectionSprite.Visible = false;
        
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
    
    private Vector2 IndexToPosition(Vector2I index)
    {
        return new Vector2(index.X, index.Y) * Spacing;
    }
    
}
