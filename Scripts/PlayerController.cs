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

    // variable global
    private Global _global;


    // metodos
    public override void _Ready()
    {
        // inicializar componentes
        _animations = GetNode<AnimatedSprite2D>("Animations");
        _global = GetNode<Global>("/root/Global");
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
        
        // TODO: accion de hit

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
        // colision con enemigos
        if(body.IsInGroup("Enemy"))
        {
            GD.Print("Hit by enemy");
            _global.lifePlayer -= 1;
            _isHit = true;            

            if(Position.X < body.Position.X)
            {
                GD.Print("Hit by enemy from left");
            }
            else
            {
                GD.Print("Hit by enemy from right");
            }
        await ToSignal(GetTree().CreateTimer(0.4), "timeout");
        _isHit = false;
        }

        // recolectar monedas
        if(body.IsInGroup("Coin"))
        {
            GD.Print("Coin collected");
            _global.coins += 1;
            body.QueueFree();
        }

        // recoletar vidas
        if(body.IsInGroup("Life"))
        {
            GD.Print("Life collected");
            _global.lifePlayer += 1;
            body.QueueFree();
        }
    }


}
