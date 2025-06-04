using Godot;
using System;
using System.Collections.Generic;
using PuzzleFight.Common;

namespace PuzzleFight.Nodes;

public partial class BoardNode : Node2D
{
    [Export] public int Width { get; set; } = 5;
    [Export] public int Height { get; set; } = 5;
    
    [Export] public float Spacing { get; set; } = 120f;
    
    [Export] Sprite2D SelectionSprite { get; set; }
    
    [Export] float DropTime { get; set; } = 0.1f;
    [Export] float SwapTime { get; set; } = 0.25f;

    [Export] Godot.Collections.Array<Node> Participants { get; set; } = new Godot.Collections.Array<Node>();
    public Board Board { get; private set; }

    List<IParticipant> _participants = new();
    int _currentParticipant = 0;
    
    private IParticipant CurrentParticipant => _participants[_currentParticipant];

    private Vector2I? _selected;
    

    private Array2D<Sprite2D> _sprites;
    private Stack<Sprite2D> _removedSprites = new();
    private Vector2 _dropOrigin;
    private bool _endTurn;

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
            if (local.X < 0 || local.Y < 0 || local.X > Width * Spacing || local.Y > Height * Spacing)
            {
                return;
            }
            var x = (int)Math.Floor(local.X / Spacing);
            var y = (int)Math.Floor(local.Y / Spacing);
            if (_selected == null)
            {
                _selected = new Vector2I(x, y);
                SelectionSprite.Position = new Vector2(x, y) * Spacing;
                SelectionSprite.Visible = true;
            }
            else
            { 
                StartSwap(_selected.Value, new Vector2I(x, y));
                _selected = null;
                SelectionSprite.Visible = false;
            }            
        }
    }

    public override void _Ready()
    {
        Board = new Board(Width, Height);
        _sprites = new Array2D<Sprite2D>(Width, Height);
     
        _selected = null;
        SelectionSprite.Visible = false;

        _dropOrigin = new Vector2(0, -Height * Spacing);

        for (var i = 0; i < Width * Height; i++)
        {
            var type = StoneTypeEnum.GemBlue;
            var sprite = new Sprite2D();
            sprite.Texture = GD.Load<Texture2D>($"res://assets/gems/{type}.png");
            sprite.Visible = false;
            AddChild(sprite);
            _removedSprites.Push(sprite);
        }
        RefreshBoard();
        
        foreach (var participant in Participants)
        {
            if (participant is IParticipant p)
            {
                _participants.Add(p);
                p.Setup(this);
            }
        }
        
    }

    void RefreshBoard()
    {
        Board.RefreshBoard();
        var mainTween = CreateTween();
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
                var type = Board.Data[x, y];
                var sprite = _removedSprites.Pop();
                sprite.Texture = GD.Load<Texture2D>($"res://assets/gems/{type}.png");
                sprite.Visible = true;
                sprite.Scale = new Vector2(1, 1);
                var pos = new Vector2(x, y) * Spacing;
                _sprites[x,y] = sprite;
                sprite.Position = _dropOrigin + pos;
                columnTween.TweenProperty(sprite, "position", pos, DropTime * Height).SetDelay(delay + 0.05 * (Height - y));
                //GD.Print("from", sprite.Position, "to", pos);
            }
            dropTween.TweenSubtween(columnTween);
        }
        
        mainTween.TweenSubtween(dropTween);
        mainTween.TweenCallback(Callable.From(AfterBoardUpdate));
    }

    // Performs animations for swap and finalizes board when animations are done
    public void StartSwap(Vector2I source, Vector2I target)
    {
        if (Board.IsValid(source, target))
        {
            // On valid swap, call EndSwap as the next step
            GD.Print($"valid swap start {source} {target}");
            DoSwap(source, target, Callable.From(() =>
                {
                    EndSwap(source, target);
                }
            ));
        }
        else
        {
            // On invalid swap, just swap the sprites back again
            DoSwap(source, target, Callable.From(() =>
                {
                    GD.Print($"invalid swap start {source} {target}");
                    DoSwap(source, target, new Callable());
                }
            ));
        }
    }
    
    private void EndSwap(Vector2I source, Vector2I target)
    {
        Board.Swap(source, target);
        AfterBoardUpdate();
    }

    private void AfterBoardUpdate()
    {
        var (matchData,matches)  = Board.GetMatches();
        
        if (matchData.Count > 0)
        {
            CurrentParticipant.DidMatch(matchData);
            DoRemove(matches);
            return;
        }


        if (Board.GetAllMoves().Count == 0)
        {
            DoRemoveAll();
            return;
        }

        // Neither of the above hanppened
        // Change Participant 
        _currentParticipant = (_currentParticipant + 1) % _participants.Count;
        _participants[_currentParticipant].TakeTurn();
    }

    private void DoRemoveAll()
    {
        var mainTween = CreateTween();
        var motionTween = CreateTween();
        motionTween.SetParallel(true);
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var delay = GD.Randi() % 20 * 0.1;
                var sprite = _sprites[x, y];
                var spriteTween = CreateTween();
                spriteTween.TweenProperty(sprite, "scale",  new Vector2(0, 0), DropTime).SetDelay(delay);
                spriteTween.TweenProperty(sprite, "visible", false, 0.01);
                motionTween.TweenSubtween(spriteTween);
                _removedSprites.Push(sprite);
            }
        }
        
        mainTween.TweenSubtween(motionTween);
        mainTween.TweenCallback(Callable.From(RefreshBoard));
    }

    private void RemoveMatching(Array2D<bool> oldMatches)
    {
        var matches = oldMatches.DeepCopy();
        // Updates the board with the state we are trying to match here
        Board.RemoveMatching();
        
        var mainTween = CreateTween();
        var motionTween = CreateTween();
        motionTween.SetTrans(Tween.TransitionType.Cubic);
        motionTween.SetParallel(true);
        
        // Drop current stones into gap
        // Drop new stones from reserve
        var didDrop = false;
        int columnDelay = 0;
        for (var x = 0; x < Width; x++)
        {
            if (didDrop)
            {
                didDrop = false;
                columnDelay+=2;
            }
            int delay = columnDelay;
            int dropHeight = 0;
            for (var y = Height - 1; y >= 0; y--)
            {
                if (matches[x, y])
                {
                    didDrop = true;
                    dropHeight+=1;
                }
                else if (dropHeight > 0)
                {
                    var sprite = _sprites[x, y];
                    var spriteTween = CreateTween();
                    spriteTween.SetTrans(Tween.TransitionType.Cubic);
                    spriteTween.TweenProperty(sprite, "position", new Vector2(x, y + dropHeight) * Spacing, DropTime * dropHeight)
                        .SetDelay(0.1 * delay);;
                    motionTween.TweenSubtween(spriteTween);
                    _sprites[x, y + dropHeight] = sprite;
                    ++delay;
                }
            }
            for (int j = dropHeight-1; j >= 0; --j)
            {
                var newColor = Board.Data[x, j];
                var newSprite = _removedSprites.Pop();
                newSprite.Texture = GD.Load<Texture2D>($"res://assets/gems/{newColor}.png");
                _sprites[x, j] = newSprite;

                newSprite.Visible = true;
                newSprite.Scale = new Vector2(1, 1);
                newSprite.Position = IndexToPosition(x, -dropHeight + j);
                var newPos = IndexToPosition(x,j);
                var spriteTween = CreateTween();
                spriteTween.SetTrans(Tween.TransitionType.Cubic);
                spriteTween.TweenProperty(newSprite, "position", newPos, DropTime * dropHeight).SetDelay(0.1 * delay);;
                motionTween.TweenSubtween(spriteTween);
                ++delay;
            }
        }
        mainTween.TweenSubtween(motionTween);
        mainTween.TweenCallback(Callable.From(() => AfterBoardUpdate()));
    }

    private void DoSwap(Vector2I source, Vector2I target, Callable callback)
    {
        var motion = CreateTween();
        motion.SetParallel(true);
        motion.SetTrans(Tween.TransitionType.Cubic);

        var sprite1 = _sprites[source];
        var sprite2 = _sprites[target];
        motion.TweenProperty(sprite1, "position", IndexToPosition(target), SwapTime);
        motion.TweenProperty(sprite2, "position", IndexToPosition(source), SwapTime);
        
        var mainTween = CreateTween();
        mainTween.TweenSubtween(motion);
        mainTween.TweenCallback(callback);   
        _sprites.Swap(source, target);
    }
    
    
    private void DoRemove(Array2D<bool> matches)
    {
        var mainTween = CreateTween();
        var motionTween = CreateTween();
        motionTween.SetParallel(true);
        motionTween.SetTrans(Tween.TransitionType.Cubic);
        int count = 0;
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (matches[x, y])
                {
                    var sprite = _sprites[x, y];
                    var spriteTween = CreateTween();
                    spriteTween.TweenProperty(sprite, "scale",  new Vector2(0, 0), DropTime).SetDelay(0.1 * count);
                    motionTween.TweenSubtween(spriteTween);
                    _removedSprites.Push(sprite);
                    count++;
                }                
            }
        }
        mainTween.TweenSubtween(motionTween);
        mainTween.TweenCallback(Callable.From(() => RemoveMatching(matches)));
    }

    
    private Vector2 IndexToPosition(Vector2I index)
    {
        return new Vector2(index.X, index.Y) * Spacing;
    }
    
    private Vector2 IndexToPosition(int x, int y)
    {
        return new Vector2(x, y) * Spacing;
    }
    
}
