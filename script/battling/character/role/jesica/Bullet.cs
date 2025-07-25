using Godot;
using System;

public partial class Bullet : Node2D
{


    public float Speed = 1200f;


    public float Lifetime = 0.5f; // 子弹存在时间(S)

    public Vector2 _direction = Vector2.Zero;

    public double atkVal;

    private float _timer = 0f;
    public bool ready_ = false;
    private GpuParticles2D gpuParticles2D;

    public override void _Ready()
    {
        gpuParticles2D = GetNode<GpuParticles2D>("GPUParticles2D");
        
    }
    public override void _PhysicsProcess(double delta)
    {
        // 移动子弹
        if (ready_ == true)
        {
            gpuParticles2D.Emitting = true;
        }
        Rotation = _direction.Angle();
        GlobalPosition += _direction * Speed * (float)delta;
        //生命周期计时
        _timer += (float)delta;
        if ((_timer >= Lifetime && ready_ == false )|| _timer >= 3)
        {
            QueueFree(); // 超过生命周期后删除子弹
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        //对实体造成伤害
        if (body is EnemyBase enemy && ready_)
        { 
            //技能伤害
            enemy.Attacked(atkVal, 0);
            return;
        }
        if (body is EnemyBase enemy1 && !ready_)
        {
            //普通伤害
            enemy1.Attacked(atkVal, 0);
            QueueFree();
        }
    }
}
