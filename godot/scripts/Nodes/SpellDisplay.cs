using Godot;
using PuzzleFight.Spells;

namespace PuzzleFight.Nodes;

public partial class SpellDisplay : Node
{
    [Export] Label _label;
    [Export] TextureButton _button;
    
    private Spell _spell;

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

    public override void _Ready()
    {
        _button.Pressed += () => _spell.Cast();
    }

    public override void _Process(double delta)
    {
        _button.Disabled = !_spell.CanCast();
    }
    
    
    
}
