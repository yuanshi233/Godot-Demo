using Godot;
using System;

public partial class Role : Node
{
	public Vector2 pos { set; get; }
	public Global.State state { set; get; }
	[Export] public PackedScene CardCScene { get; set; }
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
	public CardC cardC;
	protected double t = 0;
	protected double t1 = 0;
	public void CardCInit(int id)
	{
		CardC.Instantiate(CardCScene, id);
	}
	protected void UpdateCardC()
	{
		cardC.HP = HP;
		cardC.CD= t / SpeedSkill;
	}

}
