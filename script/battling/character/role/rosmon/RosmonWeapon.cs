using Godot;

public partial class RosmonWeapon : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Vector2 _direction;
	private Sprite2D Sprite_3_0;
	private Sprite2D Sprite_3_1;
	private Sprite2D Sprite_3_2;
	private Sprite2D Sprite_3_3;

	private Sprite2D Sprite_3_0_;
	private Sprite2D Sprite_3_1_;
	private Sprite2D Sprite_3_2_;
	private Sprite2D yz;
	private CollisionShape2D collisionShape2D;

	private PointLight2D pointLight2D;
	private GpuParticles2D gpuParticles2D;
	private AudioStreamPlayer2D audioStreamPlayer2D;
	private float Speed = 3000f;
	public Vector2 pos_;
	public double atkVal;
	public double SkillVal;
	public int way = 0;
	private bool state = false;
	public override void _Ready()
	{

		if (way == 0)
		{
			GetNode<Node2D>("normal").Visible = true;
			GetNode<Node2D>("skill").Visible = false;

			GlobalPosition = new Vector2(pos_.X - 300, pos_.Y - 300);
			_direction = GlobalPosition.DirectionTo(pos_);

			yz = GetNode<Sprite2D>("normal/Sprite2D");
			audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("normal/AudioStreamPlayer2D");
			collisionShape2D = GetNode<CollisionShape2D>("normal/Area2D/CollisionShape2D");
			Sprite_3_0 = GetNode<Sprite2D>("normal/Sprite_3_0");
			Sprite_3_1 = GetNode<Sprite2D>("normal/Sprite_3_1");
			Sprite_3_2 = GetNode<Sprite2D>("normal/Sprite_3_2");
			Sprite_3_3 = GetNode<Sprite2D>("normal/Sprite_3_3");
			pointLight2D = GetNode<PointLight2D>("normal/PointLight2D");
			gpuParticles2D = GetNode<GpuParticles2D>("normal/GPUParticles2D");

			Sprite_3_0.Visible = false;
			yz.Visible = false;
			collisionShape2D.Disabled = true;
			return;
		}
		if (way == 1)
		{
			GetNode<Node2D>("normal").Visible = false;
			GetNode<Node2D>("skill").Visible = true;

			GlobalPosition = new Vector2(pos_.X, pos_.Y - 300);
			_direction = GlobalPosition.DirectionTo(pos_);

			yz = GetNode<Sprite2D>("skill/Sprite2D");
			audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("skill/AudioStreamPlayer2D");
			collisionShape2D = GetNode<CollisionShape2D>("skill/Area2D/CollisionShape2D");
			Sprite_3_0 = GetNode<Sprite2D>("skill/Sprite_3_0");
			Sprite_3_1 = GetNode<Sprite2D>("skill/Sprite_3_1");
			Sprite_3_2 = GetNode<Sprite2D>("skill/Sprite_3_2");

			Sprite_3_0_ = GetNode<Sprite2D>("skill/Sprite_3_0_");
			Sprite_3_1_ = GetNode<Sprite2D>("skill/Sprite_3_1_");
			Sprite_3_2_ = GetNode<Sprite2D>("skill/Sprite_3_2_");

			Sprite_3_3 = GetNode<Sprite2D>("skill/Sprite_3_3");
			pointLight2D = GetNode<PointLight2D>("skill/PointLight2D");
			gpuParticles2D = GetNode<GpuParticles2D>("skill/GPUParticles2D");

			Sprite_3_0.Visible = false;
			yz.Visible = false;
			collisionShape2D.Disabled = true;
			return;
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private float time1 = 0f;
	private bool ready = false;

	public override void _Process(double delta)
	{
		if (GlobalPosition.X >= pos_.X && GlobalPosition.Y >= pos_.Y)
		{
			ready = true;
			if (way == 0)
			{
				yz.Visible = true;
				Sprite_3_3.Visible = false;
				Sprite_3_1.Visible = false;
				Sprite_3_2.Visible = false;
				pointLight2D.Visible = false;
				if (!gpuParticles2D.OneShot)
				{
					gpuParticles2D.OneShot = true;
					gpuParticles2D.Emitting = true;
					collisionShape2D.Disabled = false;
					audioStreamPlayer2D.Play();
				}
				time1 += (float)delta;
				Sprite_3_0.Visible = true;
				if (time1 >= 0.8)
					QueueFree();

				return;
			}
			if (way == 1)
			{
				yz.Visible = true;
				Sprite_3_3.Visible = false;
				Sprite_3_1.Visible = false;
				Sprite_3_2.Visible = false;

				Sprite_3_1_.Visible = false;
				Sprite_3_2_.Visible = false;
				Sprite_3_0_.Visible = true;

				pointLight2D.Visible = false;
				if (!gpuParticles2D.OneShot)
				{
					gpuParticles2D.OneShot = true;
					gpuParticles2D.Emitting = true;
					collisionShape2D.Disabled = false;
					audioStreamPlayer2D.Play();
				}
				time1 += (float)delta;
				Sprite_3_0.Visible = true;
				if (time1 >= 0.8)
					QueueFree();

				return;
			}

		}
		else
		{
			GlobalPosition += _direction * Speed * (float)delta;
		}
	}
	private ulong _lastBodyId = 0;
	private void OnBodyEntered(Node2D body)
	{
		if (!ready)
			return;
		ulong currentId = body.GetInstanceId();
		if (currentId == _lastBodyId)
			return;
		//对实体造成伤害
		if (body is EnemyBase enemy && way == 1)
		{
			//技能伤害
			enemy.Attacked(SkillVal, 0);
			_lastBodyId = currentId;
			return;
		}
		if (body is EnemyBase enemy1 && way == 0)
		{
			//普通伤害
			enemy1.Attacked(atkVal, 0);
			_lastBodyId = currentId;
			return;
		}
	}

}
