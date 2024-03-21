using Godot;
using System;

public partial class UiController : Control
{
    // variable global
    private Global _global;

    // variable nodo hijo
    public Label lifeLabel;

    public override void _Ready()
    {
        // inicializar componentes
        _global = GetNode<Global>("/root/Global");
        lifeLabel = GetNode<Label>("HBoxContainer/Label");
    }

    public override void _Process(double delta)
    {
        // actualizar la vida del jugador en GUI
        int life = _global.lifePlayer;
        lifeLabel.Text = $"Life: {life}";
    }
}
