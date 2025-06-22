using Godot;
using PuzzleFight.Nodes;

/// <summary>
/// Deals with Popup Text messages during play
/// </summary>
public partial class TextHandler : Node
{
    [Export] public int HMargin = 80;
    [Export] public int VMargin = 80;
    private RandomNumberGenerator _rg = new();
    
    Vector2 RandomPosition()
    {
        var size = DisplayServer.WindowGetSize();
        return new Vector2(_rg.RandfRange(HMargin, size.X-HMargin),
            _rg.RandfRange(VMargin, size.Y-VMargin));
    }

    public void OnEmitText(string text)
    {
        var node = ResourceLoader.Load<PackedScene>($"res://scenes/screen_text.tscn").Instantiate<ScreenText>();
        node.Visible = true;
        node.Text = text;
        node.Position = RandomPosition();
        
        GD.Print($"Text {text} At {node.Position}");
        
        AddChild(node);
    }
}
