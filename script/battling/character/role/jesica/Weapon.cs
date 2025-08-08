using Godot;
using System;

public partial class Weapon : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Node2D issue;
	private PackedScene BulletScene;
	private static float shootTime { get; set; } = 0.2f;
	private static float skillSpeed { get; set; } = 5f;
	private GpuParticles2D gpuParticles2D;
	private AudioStreamPlayer2D audioNor2D;
	private AudioStreamPlayer2D audioBig2D;

	public override void _Ready()
	{
		issue = GetNode<Node2D>("issue");
		gpuParticles2D = GetNode<GpuParticles2D>("GPUParticles2D");
		gpuParticles2D.Lifetime = shootTime - 0.1;
		audioNor2D = GetNode<AudioStreamPlayer2D>("AudioNor2D");
		audioBig2D = GetNode<AudioStreamPlayer2D>("AudioBig2D");
		BulletScene = GD.Load<PackedScene>("res://scene/character/role/jesica/bullet.tscn");
		
	}

	public static void _Init(double st, double sp)
	{
		shootTime = (float)st;
		skillSpeed = (float)sp;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}


	private void shoot(int type, Double val, Vector2 pos)
	{
		var instance = BulletScene.Instantiate() as Bullet;
		instance.pos = issue.GlobalPosition;
		instance._direction = issue.GlobalPosition.DirectionTo(pos);
		instance.atkVal = val;
		gpuParticles2D.Emitting = true;
		GetTree().Root.AddChild(instance);
		if (type == 1)
		{
			audioBig2D.Play();
			instance.ready_ = true;
		}
		else
		{
			audioNor2D.Play();
		}
	}
}
