using Godot;
using System;

public partial class RoleState : Node
{
    public Vector2 pos { set; get; }
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

}
