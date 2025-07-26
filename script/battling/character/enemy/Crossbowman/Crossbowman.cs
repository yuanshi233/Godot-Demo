using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

public partial class Crossbowman : EnemyBase
{

	[Export] public double SpeedMove { get; set; } = 100;
	[Export] public float Acceleration { get; set; } = 15.0f;
	[Export] public float Friction { get; set; } = 10.0f;

	[Export] public double HP { get; set; } = 80;
	[Export] public double AtkVal { get; set; } = 30;
	[Export] public double PhyDef { get; set; } = 0;
	[Export] public double SpellDef { get; set; } = 0;

	[Export] private PackedScene arrow;
	private Node2D issue;

	private double hp;
	private ProgressBar hpBar;
	private AnimatedSprite2D animatedSprite2D;
	private List<Node> bodyMelee = new();
	private List<Node> bodyRemote = new();

	//0正常 1近战攻击后摇 2远程攻击后摇 3死亡后摇
	private int sta = 0;

	public override void _Process(double delta)
	{
		if (sta != 0)
			return;
		if (hp < 0)
		{
			animatedSprite2D.Play("die");
			sta = 3;
			return;
		}
		if (bodyMelee.Count > 0)
		{
			sta = 1;
			animatedSprite2D.Play("combat");
			return;
		}
		if (bodyRemote.Count > 0)
		{
			sta = 2;
			animatedSprite2D.Play("attack");
			return;
		}

		Vector2 pos = (Player.player.role.GlobalPosition - GlobalPosition).Normalized();
		HandleMovement(pos);
		HandleFlipAndAnimation(pos);
	}
	//近
	private void OnBodyEnteredMelee(Node2D body)
	{
		if (body is RoleBase)
			bodyMelee.Add(body);
	}
	private void OnBodyExitedMelee(Node2D body)
	{
		if (body is RoleBase)
			bodyMelee.Remove(body);
	}
	//远
	private void OnBodyEnteredRemote(Node2D body)
	{
		if (body is RoleBase)
			bodyRemote.Add(body);
	}
	private void OnBodyExitedRemote(Node2D body)
	{
		if (body is RoleBase)
			bodyRemote.Remove(body);
	}

	public override void Reset()
	{
		sta = 0;
		bodyMelee.Clear();
		bodyRemote.Clear();
		hpBar = GetNode<ProgressBar>("HpBar");
		issue = GetNode<Node2D>("AnimatedSprite2D/issue");
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		hp = HP;
		hpBar.MaxValue = HP;
		hpBar.Value = HP;
	}

	private void AniFinished()
	{
		if (sta == 1)
		{
			if (bodyMelee.Count > 0)
			{
				((RoleBase)bodyMelee[0]).Attacked(AtkVal, 0);
				//bodyMelee.Remove(bodyMelee[0]);
			}
			sta = 0;
			return;
		}
		if (sta == 2)
		{
			if (bodyRemote.Count > 0)
			{
				//bodyMelee.Remove(bodyMelee[0]);
				var a = arrow.Instantiate<Arrow>();
				a.GlobalPosition = issue.GlobalPosition;
				a.atkVal = AtkVal;
				a._direction = issue.GlobalPosition.DirectionTo(new Vector2(
					((RoleBase)bodyRemote[0]).GlobalPosition.X,
					((RoleBase)bodyRemote[0]).GlobalPosition.Y - 25));
				GetTree().Root.AddChild(a);
			}
			sta = 0;
			return;
			
		}
		if (sta == 3)
		{
			EnemyManager.enemyManager.OnEnemyDie(this);
			return;
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
		hp -= sum;
		hpBar.Value = hp;
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
			animatedSprite2D.Offset = inputVector.X < 0 ? new Vector2(-33, -53) : new Vector2(36, -53);
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
}
