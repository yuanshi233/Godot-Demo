using Godot;
using System;

public partial class Arrow : Area2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public float Speed = 500f;
	[Export] public float Lifetime = 2;

	public double atkVal = 0;
	public Vector2 _direction = Vector2.Zero;
	private float _timer = 0;
	public override void _Ready()
	{

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
    {

        //移动
		Rotation = _direction.Angle();
        GlobalPosition += _direction * Speed * (float)delta;
        //生命周期计时
        _timer += (float)delta;
        if (_timer >= Lifetime || _timer >= 3)
        {
            QueueFree(); // 超过生命周期后删除
        }
    }

    private void OnBodyEntered(Node2D body)
    {
		GD.Print(body.Name);
        //对实体造成伤害
		if (body is RoleBase role)
		{
			
			role.Attacked(atkVal, 0);
			QueueFree();
		}
    }
}
