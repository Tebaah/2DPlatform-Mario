using Godot;
using System;

public partial class Fps : BoxContainer
{
    private Label framePerSeconds;

    public override void _Ready()
    {
        framePerSeconds = GetNode<Label>("Label");
    }

    public override void _Process(double delta)
    {
        ShowFps();
    }

    public void ShowFps()
    {
        var fps = Engine.GetFramesPerSecond();

        framePerSeconds.Text = $"fps: {fps}";
    }
}
