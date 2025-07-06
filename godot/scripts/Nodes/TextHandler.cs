using Godot;
using PuzzleFight.Nodes;

/// <summary>
/// Deals with Popup Text messages during play
/// </summary>
public partial class TextHandler : Node
{
    public enum Position
    {
        WholeScreen = 0,
        Left,
        Right, 
    }
    
    [Export] public int HMargin = 80;
    [Export] public int VMargin = 80;
    private RandomNumberGenerator _rg = new();
    
    Vector2 RandomPosition(Position position = Position.WholeScreen)
    {
        var size = DisplayServer.WindowGetSize();
        switch (position)
        {
            case Position.Left:
                return new Vector2(_rg.RandfRange(HMargin, (float)(size.X / 2.0) - HMargin),
                    _rg.RandfRange(VMargin, size.Y - VMargin));
            case Position.Right:
                return new Vector2(_rg.RandfRange((float)(size.X / 2.0) + HMargin, (float)(size.X / 2.0) - HMargin),
                    _rg.RandfRange(VMargin, size.Y - VMargin));
            default:
                return new Vector2(_rg.RandfRange(HMargin, size.X-HMargin),
                    _rg.RandfRange(VMargin, size.Y-VMargin));
        }
    }

    public void OnEmitText(string text, int position)
    {
        var node = ResourceLoader.Load<PackedScene>($"res://scenes/screen_text.tscn").Instantiate<ScreenText>();
        node.Visible = true;
        node.Text = text;
        if (position > 2) position = 0;
        node.Position = RandomPosition((Position)position);
        
        GD.Print($"Text {text} At {node.Position}");
        
        AddChild(node);
    }
}
