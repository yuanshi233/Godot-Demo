using Godot;
using System;
using System.Diagnostics;

public partial class RoleSeleUI : Control
{
	public static RoleSeleUI Instance;
	// Called when the node enters the scene tree for the first time.
	public int len = 0;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Instance = this;
	}

	private void BackUserUI()
	{
		len = 0;
		var scene = GD.Load<PackedScene>("res://scene/UI/user_ui.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}
	private void OnButtonEnter()
	{
		
	}
	[Export]
	public HBoxContainer containerB;
	[Export]
	public HBoxContainer containerA;

}
