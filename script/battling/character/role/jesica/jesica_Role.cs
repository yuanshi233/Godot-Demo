using Godot;
using System;

public partial class jesica_Role : RoleBase
{
	[Signal] public delegate void ShootEventHandler(int type, double val, Vector2 pos);

	private Sprite2D _sprite;
	private AnimatedSprite2D animatedSprite2D;
	private Node2D weaponNode;
	private Node cardC_;
	private ProgressBar cardC_HpBar;
	private ProgressBar cardC_CdBar;
	private Sprite2D sprite2D;



	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		weaponNode = GetNode<Node2D>("weaponNode2D");
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		Weapon._Init(state.SpeedAtk, state.SpeedSkill);
	}

	private bool sta = false;
	private bool skillReady = false;

	public override void _Process(double delta)
	{
		if (state.HP <= 0)
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
		if (t >= state.SpeedSkill)
		{
			t = state.SpeedSkill;
			skillReady = true;
		}
		if (IsPresent)
		{
			Hud.Instance.CD_Bar.Value = t / state.SpeedSkill;
		}
		t1 = t1 >= state.SpeedAtk ? state.SpeedAtk : t1;
		UpdateCardC();
	}


	private void HandleFlipAndAnimatioWeapon()
	{
		//翻转
		var Vec = state.pos;
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
			Velocity = Velocity.Lerp(inputVector * (int)state.SpeedMove, (float)(state.Acceleration * GetProcessDeltaTime()));
			MoveAndSlide();
		}
		else
		{
			//没有输入时施加摩擦力
			Velocity = Velocity.Lerp(Vector2.Zero, (float)(state.Friction * GetProcessDeltaTime()));
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

	public void SetPointPos(Vector2 _pos)
	{
		state.pos = _pos;
	}
	public override void Attack(int type)
	{
		if (sta)
			return;
		if (skillReady)
		{
			//技能1
			EmitSignal(SignalName.Shoot, 1, state.SkillVal, state.pos);
			skillReady = false;
			t = 0;
		}
		if (!skillReady && state.SpeedAtk <= t1)
		{
			//普通
			EmitSignal(SignalName.Shoot, 0, state.AtkVal, state.pos);
			t1 = 0;
		}
	}
	public override void Move(Vector2 inputVector)
	{
		if (sta)
			return;
		//处理移动和动画
		HandleMovement(inputVector);
		//处理翻转和动画
		HandleFlipAndAnimation(inputVector);
		HandleFlipAndAnimatioWeapon();
	}

	public void _on_animation_finished()
	{
		RoleManager.roleManager.RoleDie();
		UpdateCardC();
	}
	public override void _ExitTree()
	{

	}

}
