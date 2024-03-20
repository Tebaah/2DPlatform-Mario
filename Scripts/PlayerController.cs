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

    // variables para el estado del personaje
    private bool _isHit = false;


    // metodos
    public override void _Ready()
    {
        // inicializar componentes
        _animations = GetNode<AnimatedSprite2D>("Animations");
    }
    public override void _PhysicsProcess(double delta)
    {
        
        Vector2 velocity = Velocity;

        // accion de movimiento en eje x
        float direction = Input.GetAxis("left", "right");
        velocity.X = direction * speed;

        // agregamos la gravedad
        if(!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        // accion de salto
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = -jumpForce;
        }
        
        // accion de hit
        if(_isHit)
        {
            velocity.Y = -50;
            velocity.X = direction * -750; // TODO crear variables para el hit
        }


        // actualizamos la animacion
        UpdateSpriteRender(velocity.Y, velocity.X);

        Velocity = velocity;
        MoveAndSlide();
    
    }

    private void UpdateSpriteRender(float VelocityY, float VelocityX)
    {
        bool walking = VelocityX != 0;
        bool jumping = VelocityY != 0;

        if(walking && IsOnFloor())
        {
            _animations.Play("walk");
            _animations.FlipH = VelocityX < 0;
        }
        else
        {
            _animations.Play("idle");
        }
        if(jumping)
        {
            _animations.Play("jump");
            
        }
        if(_isHit)
        {
            _animations.Play("hit");
        }
    }

    private async void OnAreaBodyEntered(Area2D body)
    {
        if(body.IsInGroup("Enemy"))
        {
            GD.Print("Hit by enemy");
            _isHit = true;            
        }
        await ToSignal(GetTree().CreateTimer(0.4), "timeout");
        _isHit = false;
    }

}
