using Godot;
using System;

namespace PuzzleFight.Nodes;

public partial class ScreenText : Node2D
{
    [Export] public Label Label;
    [Export] public float TransitionTime = .75f;
    

    public string Text
    {
        get => Label.Text;
        set => Label.Text = value;
    }
    
    public override void _Ready()
    {
        Visible = true;

        var main = CreateTween();
        main.SetParallel(false);
        var animation = CreateTween();
        Scale = Vector2.One;
        animation.TweenProperty(this, "scale", Vector2.Zero, TransitionTime);
        main.TweenSubtween(animation);
        main.TweenCallback(Callable.From(QueueFree));
    }
}
