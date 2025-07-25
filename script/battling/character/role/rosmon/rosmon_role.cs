using Godot;
using System;

public partial class rosmon_role : RoleBase
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene CardC;
	[Export] public double SpeedMove { get; set; } = 200;
	[Export] public float Acceleration { get; set; } = 15.0f;
	[Export] public float Friction { get; set; } = 10.0f;

	[Export] public double HP { get; set; } = 100;
	[Export] public double AtkVal { get; set; } = 50;
	[Export] public double SkillVal { get; set; } = 100;
	[Export] public double PhyDef { get; set; } = 0;
	[Export] public double SpellDef { get; set; } = 0;
	[Export] public double SpeedAtk { get; set; } = 0;
	[Export] public double SpeedSkill { get; set; } = 5;
	private Global.State state;
	private AnimatedSprite2D animatedSprite2D;
	private Sprite2D sprite2D;
	private PackedScene jujian;
	private bool skillReady;
	public override Vector2 pos { set; get; }
	private Node cardC_;
	private ProgressBar cardC_HpBar;
	private ProgressBar cardC_CdBar;

	private int sta;        //1死亡2后摇

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("body/AnimatedSprite2D");
		sprite2D = GetNode<Sprite2D>("body/Sprite2D");
		jujian = GD.Load<PackedScene>("res://scene/character/role/rosmon/rosmon_weapon.tscn");
	}

	private double t1, t;

	public override void _Process(double delta)
	{
		if (sta == 1)
			return;
		if (HP <= 0)
		{
			sta = 1;
			GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
			animatedSprite2D.Play("die");
			return;
		}

		t1 += delta;
		t += delta;
		if (t >= SpeedSkill)
		{
			t = SpeedSkill;
			skillReady = true;
		}
		if (state == Global.State.HUD)
		{
			Hud.Instance.CD_Bar.Value = t / SpeedSkill;
		}
		t1 = t1 >= SpeedAtk ? SpeedAtk : t1;
		UpdateCardC(delta);
	}
	public override void CardCInit(int id)
	{
		cardC_ = CardC.Instantiate();
		cardC_HpBar = cardC_.GetNode<ProgressBar>("HpBar");
		cardC_CdBar = cardC_.GetNode<ProgressBar>("CdBar");
		cardC_.GetNode<Label>("id").Text = "[" + id.ToString() + "]";
		Hud.RolePane.AddChild(cardC_);
		//RoleManager.RolePane.CallDeferred("add_child", cardC_);
	}

	private void UpdateCardC(double delta)
	{
		cardC_HpBar.Value = HP;
		cardC_CdBar.Value = t / SpeedSkill;
	}

	private void HandleMovement(Vector2 inputVector)
	{
		if (inputVector != Vector2.Zero)
		{
			//加速到目标速度
			Velocity = Velocity.Lerp(inputVector * (int)SpeedMove, (float)(Acceleration * GetProcessDeltaTime()));
			MoveAndSlide();
		}
		else
		{
			//没有输入时施加摩擦力
			Velocity = Velocity.Lerp(Vector2.Zero, (float)(Friction * GetProcessDeltaTime()));
			MoveAndSlide();
		}
	}

	private void HandleFlipAndAnimation(Vector2 inputVector)
	{
		if (inputVector.X != 0)
		{
			//翻转
			animatedSprite2D.FlipH = inputVector.X < 0;
			sprite2D.Offset = inputVector.X < 0 ? new Vector2(-40, 0) : new Vector2(0, 0);

		}

		//根据速度播放动画
		if (Velocity.Length() > 5)
		{
			animatedSprite2D.Play("walk");
		}
		else
		{
			animatedSprite2D.Play("idle");
		}
	}

	//被攻击，0物理，1法术
	public override void Attacked(double value, int type)
	{
		double sum = 1.0;
		if (type == 0)
		{
			sum = (value - PhyDef) <= 0 ? 1 : value - PhyDef;
		}
		else if (type == 1)
		{
			sum = (value - SpellDef) <= 0 ? 1 : value - SpellDef;
		}
		else if (type == 2)
		{
			sum = -value;
		}
		HP -= sum;
		if (state == Global.State.HUD)
		{
			Hud.Instance.HP_Bar.Value = HP;
		}
	}
	public override void Move(Vector2 inputVector)
	{
		if (sta == 2)
			return;
		//处理移动和动画
		HandleMovement(inputVector);
		//处理翻转和动画
		HandleFlipAndAnimation(inputVector);
	}
	//用于role权限
	public override void SetState(Global.State stat)
	{
		state = stat;
		if (stat == Global.State.HUD)
		{
			Hud.Instance.HP_Bar.Value = HP;
		}
	}
	public void SetPointPos(Vector2 _pos)
	{
		pos = _pos;
	}
	public override void Attack(int type)
	{
		if (sta == 2)
			return;
		if (type == 1 && skillReady)
		{
			//技能
			//翻转
			sta = 2;
			animatedSprite2D.Play("skill");
			animatedSprite2D.FlipH = pos.X < Position.X;
			sprite2D.Offset = pos.X < Position.X ? new Vector2(-40, 0) : new Vector2(0, 0);
			RosmonWeapon jj = jujian.Instantiate() as RosmonWeapon;
			jj.GlobalPosition = pos;
			jj.way = 1;
			jj.pos_ = pos;
			GetTree().Root.AddChild(jj);
			skillReady = false;
			t = 0;
			return;
		}
		if (type == 0 && SpeedAtk <= t1)
		{
			//普攻
			//翻转
			sta = 2;
			animatedSprite2D.Play("attack");
			animatedSprite2D.FlipH = pos.X < Position.X;
			sprite2D.Offset = pos.X < Position.X ? new Vector2(-40, 0) : new Vector2(0, 0);
			RosmonWeapon jj = jujian.Instantiate() as RosmonWeapon;
			jj.GlobalPosition = pos;
			jj.way = 0;
			jj.pos_ = pos;
			GetTree().Root.AddChild(jj);
			t1 = 0;
			return;
		}
	}
	public void _on_animation_finished()
	{
		if (sta == 1)
			QueueFree();
		if (sta == 2)
			sta = 0;
	}
	public override void _ExitTree()
	{
	}

}
