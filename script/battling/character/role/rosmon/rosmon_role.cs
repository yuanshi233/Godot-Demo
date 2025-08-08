using Godot;
using System;

public partial class rosmon_role : RoleBase
{
	private AnimatedSprite2D animatedSprite2D;
	private Sprite2D sprite2D;
	private PackedScene jujian;
	private bool skillReady => t >= state.SpeedSkill;
	private Node cardC_;
	private ProgressBar cardC_HpBar;
	private ProgressBar cardC_CdBar;

	public override bool IsPresent
	{
		set => IsPresent_ = this == (GetParent().GetParent().GetNode("Player") as Player).role;
		get => IsPresent_;
	}

    private int sta;        //1死亡2后摇

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		jujian = GD.Load<PackedScene>("res://scene/character/role/rosmon/rosmon_weapon.tscn");
	}

	protected override void OnDie()
	{
		if (sta != 1)
		{
			UpdateCardC();
			sta = 1;
			GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
			animatedSprite2D.Play("die");
			return;
		}
	}
	public override void _Process(double delta)
	{
		if (sta == 1)
			return;
		t1 += delta;
		t += delta;
		if (IsPresent)
		{
			Hud.CD = Math.Min(1, t / state.SpeedSkill);
		}
		t1 = Math.Min(t1, state.SpeedAtk);
		UpdateCardC();
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
			sprite2D.Offset = inputVector.X < 0 ? new Vector2(-40, 4) : new Vector2(0, 4);

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

	public override void Move(Vector2 inputVector)
	{
		if (sta == 2 || sta == 1)
			return;
		//处理移动和动画
		HandleMovement(inputVector);
		//处理翻转和动画
		HandleFlipAndAnimation(inputVector);
	}

	public void SetPointPos(Vector2 _pos)
	{
		state.pos = _pos;
	}
	public override void Attack(int type)
	{
		if (sta == 2 || sta == 1)
			return;
		if (type == 1 && skillReady)
		{
			//技能
			//翻转
			sta = 2;
			animatedSprite2D.Play("skill");
			animatedSprite2D.FlipH = state.pos.X < Position.X;
			sprite2D.Offset = state.pos.X < Position.X ? new Vector2(-40, 0) : new Vector2(0, 0);
			RosmonWeapon jj = jujian.Instantiate() as RosmonWeapon;
			jj.GlobalPosition = state.pos;
			jj.way = 1;
			jj.pos_ = state.pos;
			jj.atkVal = state.AtkVal;
			jj.SkillVal = state.SkillVal;
			//GetTree().Root.AddChild(jj);
			AddChild(jj);
			t = 0;
			return;
		}
		if (type == 0 && state.SpeedAtk <= t1)
		{
			//普攻
			//翻转
			sta = 2;
			animatedSprite2D.Play("attack");
			animatedSprite2D.FlipH = state.pos.X < Position.X;
			sprite2D.Offset = state.pos.X < Position.X ? new Vector2(-40, 0) : new Vector2(0, 0);
			RosmonWeapon jj = jujian.Instantiate() as RosmonWeapon;
			jj.GlobalPosition = state.pos;
			jj.way = 0;
			jj.pos_ = state.pos;
			jj.atkVal = state.AtkVal;
			jj.SkillVal = state.SkillVal;
			//GetTree().Root.AddChild(jj);
			AddChild(jj);
			t1 = 0;
			return;
		}
	}
	public void _on_animation_finished()
	{
		if (sta == 1)
		{
			(GetParent().GetParent().GetNode("RoleManager")  as RoleManager).RoleDie();
		}
		if (sta == 2)
			sta = 0;
	}
	public override void _ExitTree()
	{
	}

}
