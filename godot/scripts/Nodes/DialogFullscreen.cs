
#nullable enable

using Godot;
using System;
using System.Linq;
using PuzzleFight.Nodes;

namespace GodotInk;

public partial class DialogFullscreen : VBoxContainer
{
    
    private static readonly StringName ChoiceIndexMeta = "Index";
    
    private Label _storyNameLabel = null!;
    private VBoxContainer _storyText = null!;
    private VBoxContainer _storyChoices = null!;
    private ScrollContainer _scroll = null!;
    
    private string _storyPath = "";
    private InkStory? _story;
    private bool _storyStarted;

    private string _backupSave = "";

    public override void _Ready()
    {
        base._Ready();

        CustomMinimumSize = new Vector2(0.0f, 228.0f);
        
        _storyNameLabel = GetNode<Label>("Container/Left/Top/Label");
        // Initialize bottom.
        _storyText = GetNode<VBoxContainer>("Container/Left/Scroll/Margin/StoryText");
        _storyChoices = GetNode<VBoxContainer>("Container/Right/StoryChoices");
        _scroll = GetNode<ScrollContainer>("Container/Left/Scroll");

        _story = GameManager.Instance.Story;
 
        RestoreNodeState(GameManager.Instance.GameGuid.ToString());
        
        UpdateTop();
    }
    
    private void RestoreNodeState(string guid)
    {
        string scenePath = $"user://{guid}-story.tscn";
        if (ResourceLoader.Exists(scenePath))
        {
            var packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.IgnoreDeep);

            var node = packedScene.Instantiate() as VBoxContainer;

            if (node != null)
            {
                BuildChoices();
                var parent = _storyText.GetParent();
                parent.RemoveChild(_storyText);
                _storyText.QueueFree();
                _storyText = node;
                parent.AddChild(_storyText);
                _storyStarted = true;
                ContinueStory();
                if (GameManager.Instance.LastResult != "")
                {
                    FindAndExecute(GameManager.Instance.LastResult);
                }
                _scroll.ScrollVertical = (int)_scroll.GetVScrollBar().MaxValue;
                ContinueStory();
            }
        }
        else
        {
            StartStory();
        }
    }

  
    private void StoreNodeState(string guid)
    {
        string scenePath = $"user://{guid}-story.tscn";
        var packedScene = new PackedScene();
        packedScene.Pack(_storyText);
        var error = ResourceSaver.Save(packedScene, scenePath);
        if (error != Error.Ok)
        {
            GD.PrintErr(error.ToString());
        }
    }

    private void UpdateTop()
    {
        bool hasStory = _story != null;
        _storyChoices.GetParent<Control>().Visible = hasStory;
    }
    
    private void StartStory()
    {
        if (_story == null) return;

        _storyStarted = true;
        ContinueStory();
        UpdateTop();
    }

    private void StopStory()
    {
        StopStory(false);
    }

    private void StopStory(bool setStoryToNull)
    {
        _storyStarted = false;

        try
        {
            _story?.ResetState();
        }
        catch (ObjectDisposedException)
        {
            _story = null;
        }

        if (setStoryToNull)
            _story = null;

        ClearStory(true);
    }

    private void ClearStory(bool clearChoices)
    {
        RemoveAllStoryContent();
        if (clearChoices)
            RemoveAllChoices();

        UpdateTop();
    }

    private void ContinueStory()
    {
        if (_story == null) return;
        if (!_story.CanContinue) return;

        string currentText = _story.ContinueMaximally().Trim();

        if (currentText.Length > 0)
        {
            Label newLine = new()
            {
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                Text = currentText,
            };
            AddToStory(newLine);

            if (_story.CurrentTags.Count > 0)
            {
                newLine = new Label()
                {
                    AutowrapMode = TextServer.AutowrapMode.WordSmart,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = $"# {string.Join(", ", _story.CurrentTags)}",
                };
                newLine.AddThemeColorOverride("font_color", GetThemeColor("font_color_disabled", "Button"));
                AddToStory(newLine);
            }
        }

        BuildChoices();
        _backupSave = _storyStarted ? _story.SaveState() : "";

        if (GameManager.Instance.HasEncounter())
        {
            GameManager.Instance.EncounterText = currentText;
            StoreNodeState(GameManager.Instance.GameGuid.ToString());
            GameManager.Instance.DoEncounter();
        }
    }
    
    private void BuildChoices()
    {
        foreach (InkChoice choice in _story.CurrentChoices)
        {
            Button button = new() { Text = choice.Text };
            button.SetMeta(ChoiceIndexMeta, choice.Index);

            button.Connect(Button.SignalName.Pressed, Callable.From(ClickChoice));

            _storyChoices.AddChild(button);
        }
    }
    
    private void FindAndExecute(string choiceText)
    {
        // TODO maybe should use tags here rather than text (safer with internationalization)
        foreach (InkChoice choice in _story.CurrentChoices)
        {
            if (choiceText == choice.Text)
            {
                ClickChoice(choice.Index);
            }
        }        
    }


    private void ClickChoice()
    {
        if (_storyChoices.GetChildren().OfType<Button>().First(button => button.ButtonPressed) is not Button button) return;
        if (!button.HasMeta(ChoiceIndexMeta)) return;

        try
        {
            ClickChoice(button.GetMeta(ChoiceIndexMeta).As<int>());
        }
        catch
        {
            _story?.LoadState(_backupSave);
            try
            {
                ClickChoice(button.GetMeta(ChoiceIndexMeta).As<int>());
            }
            catch
            {
                StopStory(true);
            }
        }
    }

    private void ClickChoice(int idx)
    {
        if (_story == null) return;

        _story.ChooseChoiceIndex(idx);

        RemoveAllChoices();
        AddToStory(new HSeparator());

        ContinueStory();
    }

    private void AddToStory(CanvasItem item)
    {
        _storyText.AddChild(item);
        item.Owner = _storyText;
        // await ToSignal(GetTree(), "process_frame");
        // await ToSignal(GetTree(), "process_frame");
        _scroll.ScrollVertical = (int)_scroll.GetVScrollBar().MaxValue;
    }

    private void RemoveAllStoryContent()
    {
        foreach (Node n in _storyText.GetChildren())
            n.QueueFree();
    }

    private void RemoveAllChoices()
    {
        foreach (Node n in _storyChoices.GetChildren().OfType<Button>())
            n.QueueFree();
    }

    public void WhenInkResourceReimported()
    {
        StopStory(true);
    }
}

