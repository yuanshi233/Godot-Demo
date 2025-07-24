using Godot;
using System;

public partial class MainUI : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var scene = GD.Load<PackedScene>("res://scene/UI/user_ui.tscn");
		GetTree().CallDeferred("change_scene_to_packed", scene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
