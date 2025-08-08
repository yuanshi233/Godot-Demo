using Godot;

public abstract partial class EnemyBase : CharacterBody2D
{
    [Export] public double SpeedMove { get; set; } = 100;
	[Export] public float Acceleration { get; set; } = 15.0f;
	[Export] public float Friction { get; set; } = 10.0f;

	[Export] public double HP { get; set; } = 80;
	[Export] public double AtkVal { get; set; } = 30;
	[Export] public double PhyDef { get; set; } = 0;
	[Export] public double SpellDef { get; set; } = 0;
    public abstract void Attacked(double value, int type);
    public abstract void Reset();

    //public void SetState(Global.State stat);
}
