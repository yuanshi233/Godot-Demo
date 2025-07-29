using System;
using Godot;

public abstract partial class RoleBase : CharacterBody2D
{
	public bool IsDied => state.HP <= 0;
	public bool IsPresent => this == Player.player.role;
	[Export] public PackedScene CardCScene { get; set; }
	public abstract void Attack(int type);
	public abstract void Move(Vector2 inputVector);
	protected abstract void OnDie();
	[Export] public RoleState state;
	public CardC cardC;
	public double t = 0;
	public double t1 = 0;
	public enum HitType
	{
		phicial,
		spell,
		health
	}
	double CD => t / state.SpeedSkill;
	double HP => state.HP;
	public void Attacked(double value, HitType type)
	{
		state.HP -= type switch
		{
			HitType.phicial => Math.Max(value - state.PhyDef, 1),
			HitType.spell => Math.Max(value - state.SpellDef, 1),
			HitType.health => -value,
			_ => 0
		};
		Hud.HP = HP;
		if (IsDied)
		{
			OnDie();
		}
	}
	public void UpdateCardC()
	{
		cardC.HP = state.HP;
		cardC.CD = t / state.SpeedSkill;
	}

	public void UpdateHud()
	{
		Hud.HP = HP;
		Hud.CD = CD;
	}
	public CardC CardCInit(int i)
	{
		cardC = CardC.Instantiate(CardCScene, i);
		return cardC;
	}

} 
