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
    private Vector2 _dropOrigin;
    
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
                Swap(_selected.Value, new Vector2I(x, y));
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

        _dropOrigin = new Vector2(0, -Height * Spacing);
        
        var dropTween = CreateTween();
        dropTween.SetParallel(true);
        
        for (var x = 0; x < Width; x++)
        {
            var columnTween = CreateTween(); 
            columnTween.SetTrans(Tween.TransitionType.Cubic);
            columnTween.SetParallel(true);

            var delay = 0.25 * x; 
            for (var y = 0; y < Height; y++)
            {
                var type = _board.Data[x, y];
                var sprite = new Sprite2D();
                sprite.Texture = GD.Load<Texture2D>($"res://assets/gems/{type}.png");
                var pos = new Vector2(x, y) * Spacing;
                _sprites[x,y] = sprite;
                AddChild(sprite);
                
                sprite.Position = _dropOrigin + pos;
                
                columnTween.TweenProperty(sprite, "position", pos, .5f).SetDelay(delay + 0.05 * (Height - y));
                GD.Print("from", sprite.Position, "to", pos);
            }
            dropTween.TweenSubtween(columnTween);
        }
    }

    // Performs animations for swap and finalizes board when animations are done
    private void Swap(Vector2I source, Vector2I target)
    {
        var sprite1 = _sprites[source];
        var sprite2 = _sprites[target];

        var motion = CreateTween();
        motion.SetParallel(true);
        motion.SetTrans(Tween.TransitionType.Cubic);

        motion.TweenProperty(sprite1, "position", IndexToPosition(target), 0.25f);
        motion.TweenProperty(sprite2, "position", IndexToPosition(source), 0.25f);
        
        var mainTween = CreateTween();
        mainTween.TweenSubtween(motion);
        mainTween.TweenCallback(Callable.From(() =>
            {
                _board.Swap(source, target);
                _sprites.Swap(source, target);
                GD.Print("swapped {0} {1}", source, target);
            }
        ));
    }
    
    private Vector2 IndexToPosition(Vector2I index)
    {
        return new Vector2(index.X, index.Y) * Spacing;
    }
    
}
