using Godot;
using System;

public partial class RoleManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player.player.id = 0;
		Player.player.ChangeRole(Global.RoleTeam[0]);
		
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			if (Player.player.id != i)
			{
				Global.RoleTeam[i]._Process(delta);
			}
			if (Input.IsKeyPressed((Key)((int)Key.Key1 + i)) && Player.player.id != i)
			{
				Player.player.id = i;
				Player.player.ChangeRole(Global.RoleTeam[i]);
			}
		}
	}




}
