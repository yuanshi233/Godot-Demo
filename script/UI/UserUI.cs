using Godot;
using System;

public partial class UserUI : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnButtonDownStart()
	{
		var scene = GD.Load<PackedScene>("res://scene/UI/role_sele_ui.tscn");
        GetTree().ChangeSceneToPacked(scene);
	}
}
