using Godot;
using System;
using System.Globalization;

public partial class Player : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Vector2 inputVector;
	public RoleBase role;
	public int id = -1;
	public static Player player;
	private Node camera2D;
	private Node2D node2D;
	public bool _lock = false;

	public override void _Ready()
	{
		camera2D = GD.Load<PackedScene>("res://scene/UI/camera.tscn").Instantiate();
		node2D = GetNode<Node2D>("../character");
		player = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (role == null || Global.RoleTeam[id].state == Global.State.DIE || _lock)
			return;
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

	public void ChangeRole(RoleBase r)
	{
		if (role != null)
		{
			role.state = Global.RoleTeam[id].state == Global.State.DIE ? Global.State.DIE : Global.State.NUL;
			role.RemoveChild(camera2D);
			r.GlobalPosition = role.GlobalPosition;
			node2D.RemoveChild(role);
		}
		role = r;
		role.state = Global.State.HUD;
		node2D.AddChild(role);
		role.AddChild(camera2D);
	}

}
