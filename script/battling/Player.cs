using Godot;
using System;

public partial class Player : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Vector2 inputVector;
	private static RoleBase role;
	public override void _Ready()
	{
		//test
		
		var a = GD.Load<PackedScene>("res://scene/character/role/rosmon/rosmon_role.tscn").Instantiate();
		AddChild(a);
		a.AddChild(GD.Load<PackedScene>("res://scene/UI/camera.tscn").Instantiate());
		role = a as RoleBase;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		role.Move(inputVector);
		//role.SetPointPos(GetGlobalMousePosition());
		role.pos = GetGlobalMousePosition();
		if (Input.IsActionPressed("fire"))
		{
			role.Attack(0);
		}
		if (Input.IsActionPressed("skill"))
		{
			
			role.Attack(1);
		}
	}
}
