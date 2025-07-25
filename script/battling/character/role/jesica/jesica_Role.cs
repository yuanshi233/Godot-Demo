using Godot;
using System;

public partial class jesica_Role : RoleBase
{
	[Signal] public delegate void ShootEventHandler(int type, double val, Vector2 pos);

	[Export] public PackedScene CardC { get; set; }
	[Export] public double SpeedMove { get; set; } = 200;
	[Export] public float Acceleration { get; set; } = 15.0f;
	[Export] public float Friction { get; set; } = 10.0f;

	[Export] public double HP { get; set; } = 100;
	[Export] public double AtkVal { get; set; } = 10;
	[Export] public double SkillVal { get; set; } = 50;
	[Export] public double PhyDef { get; set; } = 0;
	[Export] public double SpellDef { get; set; } = 0;
	[Export] public double SpeedAtk { get; set; } = 0.2;
	[Export] public double SpeedSkill { get; set; } = 5;

	private Sprite2D _sprite;
	private AnimatedSprite2D animatedSprite2D;
	private Node2D weaponNode;
	private Node2D player;
	private Global.State state;
	//private Vector2 pos;
	public override Vector2 pos{ set; get; }
	private Node cardC_;
	private ProgressBar cardC_HpBar;
	private ProgressBar cardC_CdBar;
	private Sprite2D sprite2D;



	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("body/Sprite2D");
		animatedSprite2D = GetNode<AnimatedSprite2D>("body/AnimatedSprite2D");
		weaponNode = GetNode<Node2D>("body/weaponNode2D");
		sprite2D = GetNode<Sprite2D>("body/Sprite2D");
		player = GetNode<Node2D>("body");
		Weapon._Init(SpeedAtk, SpeedSkill);

	}

	private bool sta = false;
	private double t = 0;
	private double t1 = 0;
	private bool skillReady = false;

	public override void _Process(double delta)
	{
		if (HP <= 0)
		{
			if (sta == false)
			{
				GetNode<AudioStreamPlayer2D>("die2D").Play();
				sta = true;
				animatedSprite2D.Play("die");

			}
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
			Hud.Instance.CD_Bar.Value = t/SpeedSkill;
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
	}

	private void UpdateCardC(double delta)
	{
		cardC_HpBar.Value = HP;
		cardC_CdBar.Value = t / SpeedSkill;
	}

	private void HandleFlipAndAnimatioWeapon()
	{
		//翻转
		var Vec = pos;
		animatedSprite2D.FlipH = Vec.X < Position.X;
		sprite2D.Offset = Vec.X < 0 ? new Vector2(-15, 0) : new Vector2(0, 0);
		weaponNode.Scale = Vec.X < Position.X ? new Vector2(1, -1) : new Vector2(1, 1);
		weaponNode.LookAt(Vec);
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
			sprite2D.Offset = inputVector.X < 0 ? new Vector2(-15, 0) : new Vector2(0, 0);
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

	//被攻击，0物理，1法术, 2治疗

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
	public void SetPointPos(Vector2 _pos)
	{
		pos = _pos;
	}
	public override void Attack(int type)
	{
		if (skillReady)
		{
			//技能1
			EmitSignal(SignalName.Shoot, 1, SkillVal, pos);
			skillReady = false;
			t = 0;
		}
		if (!skillReady && SpeedAtk <= t1)
		{
			//普通
			EmitSignal(SignalName.Shoot, 0, AtkVal, pos);
			t1 = 0;
		}
	}
	public override void Move(Vector2 inputVector)
	{
		//处理移动和动画
		HandleMovement(inputVector);
		//处理翻转和动画
		HandleFlipAndAnimation(inputVector);
		HandleFlipAndAnimatioWeapon();
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
	public void _on_animation_finished()
	{
		QueueFree();
	}
	public override void _ExitTree()
	{
		
	}   
	
}
