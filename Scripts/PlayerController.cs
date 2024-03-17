using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    // variable acciones personaje
    [Export] public float speed;
    [Export] public float jumpForce;

    // variable para obtener gravedad desde la configuracion 
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    // variables para nodos hijos
    private AnimatedSprite2D _animations;



    // metodos
    public override void _Ready()
    {
        // inicializar componentes
        _animations = GetNode<AnimatedSprite2D>("Animations");
    }
    public override void _PhysicsProcess(double delta)
    {
        {
        Vector2 velocity = Velocity;

        // agregamos la gravedad
        if(!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        // accion de salto
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = -jumpForce;
            _animations.Play("jump");
        }

        // accion de movimiento en eje x
        float direction = Input.GetAxis("left", "right");
        velocity.X = direction * speed;

        if (direction != 0)
        {
            _animations.Play("walk");
            _animations.FlipH = direction < 0;
        }
        else
        {
            _animations.Play("idle");
        }

        Velocity = velocity;
        MoveAndSlide();
        }
    }

}
