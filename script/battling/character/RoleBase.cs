using System;
using Godot;

public abstract partial class RoleBase : CharacterBody2D
{
	public bool IsDied=>state.HP<=0;
	public bool IsPresent => this == Player.player.role;
	[Export] public PackedScene CardCScene { get; set; }
	public abstract void Attack(int type);
	public abstract void Move(Vector2 inputVector);
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
	public void Attacked(double value, HitType type)
	{
		state.HP -= type switch
		{
			HitType.phicial => Math.Max(value - state.PhyDef, 1),
			HitType.spell => Math.Max(value - state.SpellDef, 1),
			HitType.health => -value,
			_=>0
		};
		Hud.Instance.HP_Bar.Value = state.HP;
	}
	public void UpdateCardC()
	{
		cardC.HP = state.HP;
		cardC.CD = t / state.SpeedSkill;
	}
	public CardC CardCInit(int i)
	{
		cardC = CardC.Instantiate(CardCScene, i);
		return cardC;
	}
} 
