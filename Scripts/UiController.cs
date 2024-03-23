using Godot;
using System;

public partial class UiController : Control
{
    // variable global
    private Global _global;

    private AnimatedSprite2D _lifeAnimation;
    private Label _coinLabel;

    public override void _Ready()
    {
        // inicializar componentes
        _global = GetNode<Global>("/root/Global");
        _lifeAnimation = GetNode<AnimatedSprite2D>("BoxContainer/HBoxContainer/AnimatedSprite2D");
        _coinLabel = GetNode<Label>("BoxContainer/HBoxContainer2/Label");
    }

    public override void _Process(double delta)
    {
        // actualizar la vida del jugador en GUI
        if(_global.lifePlayer == 2)
        {
            _lifeAnimation.Play("full");
        }
        else if(_global.lifePlayer == 1)
        {
            _lifeAnimation.Play("half");
        }
        else
        {
            _lifeAnimation.Play("empty");
        }

        // actualizar las monedas del jugador en GUI
        int coins = _global.coins;
        _coinLabel.Text = $"Coins {coins}";
    }
}
