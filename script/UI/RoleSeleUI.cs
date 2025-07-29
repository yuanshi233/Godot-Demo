using Godot;
using System;
using System.Diagnostics;

public partial class RoleSeleUI : Control
{
	public static RoleSeleUI Instance;
	// Called when the node enters the scene tree for the first time.

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Instance = this;
	}

	private void BackUserUI()
	{
		Global.RoleTeam.Clear();
		var scene = GD.Load<PackedScene>("res://scene/UI/user_ui.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}
	private void OnButtonEnter()
	{
		GetTree().ChangeSceneToPacked(batScene);
	}
	[Export]
	public HBoxContainer containerB;
	[Export]
	public HBoxContainer containerA;
	[Export]
	public PackedScene batScene;

}
