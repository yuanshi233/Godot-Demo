using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Hud : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public ProgressBar HP_Bar { get; set; }
	[Export]
	public ProgressBar CD_Bar { get; set; }
	public static Hud Instance;
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
