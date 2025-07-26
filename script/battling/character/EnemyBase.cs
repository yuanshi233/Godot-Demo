using Godot;
using System;

public abstract partial class EnemyBase : CharacterBody2D
{
    
    public abstract void Attacked(double value, int type);
    public abstract void Reset();

    //public void SetState(Global.State stat);
}
