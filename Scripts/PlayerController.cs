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
    private bool _isJumping = false;

    // variable global
    private Global _global;


    // metodos
    public override void _Ready()
    {
        // inicializar componentes
        _animations = GetNode<AnimatedSprite2D>("Animations");
        _global = GetNode<Global>("/root/Global");
    }
    public override void _Process(double delta)
    {
        if(Position.Y >= 1080)
        {
            _global.isAlive = false;
        }
    }

    // actualizaciones fisicas del juego
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
        else if(IsOnFloor())
        {
            _isJumping = false;
        }

        // accion de salto
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = -jumpForce;
            _isJumping = true;
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
        // elimina a los eenmigos
        if(body.IsInGroup("Enemy") && _isJumping)
        {
            GD.Print("Hit by enemy from top");
            body.QueueFree();   
        }
        // pierde unha vida el personaje
        else if(body.IsInGroup("Enemy"))
        {
            GD.Print("Hit by enemy");
            _global.lifePlayer -= 1;
            _isHit = true;            

            // TODO: impulso en direccion contraria a la que va

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
