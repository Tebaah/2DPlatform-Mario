using Godot;
using System;

public partial class EnemyController : Area2D
{
    // variables de movimeinto
    [Export] public float speed = 100.0f;

    // variable de posicion
    [Export] public float destiny;
    private Vector2 _initialPosition;
    private bool _isMoving = true;
    // variables nodos hijos
    private AnimatedSprite2D _animationController;


    public override void _Ready()
    {
        _initialPosition = Position;
        _animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        // Movimiento del enemigo
        float move = speed * (float)delta;

        if(_isMoving)
        {
            MoveLocalX(-move);
            _animationController.Play("walk");
            _animationController.FlipH = false;

            if(Position.X <= destiny)
            {
                Position = new Vector2(destiny, Position.Y);
                _isMoving = false;
            }
        }
        else
        {
            MoveLocalX(move);
            _animationController.FlipH = true;
            if(Position.X >= _initialPosition.X)
            {
                Position = new Vector2(_initialPosition.X, Position.Y);
                _isMoving = true;
            }
        }

    }
}
