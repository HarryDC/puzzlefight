using Godot;
using PuzzleFight.Spells;

namespace PuzzleFight.Nodes;

public partial class SpellDisplay : Node
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
    }
    
    
    
}
