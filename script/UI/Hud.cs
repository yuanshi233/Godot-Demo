using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Hud : Control
{
	// Called when the node enters the scene tree for the first time.
	public static ProgressBar HP_Bar { get; set; }
	public static ProgressBar CD_Bar { get; set; }
	public override void _Ready()
	{
		HP_Bar = GetNode<ProgressBar>("HpBar");
		CD_Bar = GetNode<ProgressBar>("CdBar");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
