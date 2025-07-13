using Godot;
using PuzzleFight.Spells;

namespace PuzzleFight.Nodes;

public partial class SpellDisplay : HBoxContainer
{
    [Export] Label _label;
    [Export] TextureButton _button;
    
    private Spell _spell;
    private TextHandler _textHandler;
    
    [Export]
    public Spell Spell
    {
        get => _spell;
        set
        {
            _spell = value; 
            _label.Text = _spell.Name;
        }
    }

    [Export] public Participant Participant;

    private Panel _rollover;
    
    public override void _Ready()
    {
        _textHandler = GetNode<TextHandler>("/root/Game/TextHandler");
        _button.Pressed += ExecuteCastSpell;
    }

    private void ExecuteCastSpell()
    {
        Participant.Cast(_spell);
    }

    public override void _Process(double delta)
    {
        _button.Disabled = !_spell.CanCast(Participant.Character);

        if (_rollover != null)
        {
            // Keep the rollover item away from the mouse otherwise we will 
            // immediately receive a MousExited signal
            _rollover.Position = GetGlobalMousePosition() + new Vector2(10,10);
        }
    }

    public void OnMouseEntered()
    {
        _rollover = ResourceLoader.Load<PackedScene>("res://scenes/rollover_container.tscn").Instantiate<Panel>();
        _rollover.Position = GetGlobalMousePosition() + new Vector2(10,10);
        var label = _rollover.FindChild("RichTextLabel") as RichTextLabel;
        if (label != null)
        {
            label.Text = Spell.Description;
        }
        GetTree().GetRoot().AddChild(_rollover);
    }

    public void OnMouseExited()
    {
        // There is some issue with remove child and de
        //GetTree().GetRoot().RemoveChild(_rollover);
        _rollover.QueueFree();
        _rollover = null;
    }
    
    
}
