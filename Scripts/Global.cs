using Godot;
using System;

public partial class Global : Node
{
    // variables de jugador
    public int lifePlayer = 2;
    public int coins;

    public override void _Process(double delta)
    {
        // TODO: verificar si el jugador ha perdido
        if(lifePlayer > 2)
        {
            lifePlayer = 2;
        }
    }
}

