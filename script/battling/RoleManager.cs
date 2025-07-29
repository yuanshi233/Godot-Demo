using Godot;
using System;

public partial class RoleManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public static RoleManager roleManager;
	public override void _Ready()
	{
		Player.player.id = 0;
		Player.player.ChangeRole(Global.RoleTeam[0]);
		foreach (var role in Global.RoleTeam)
			role.UpdateCardC();
		roleManager = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private bool _lock = false;
	public override void _Process(double delta)
	{
		if (_lock)
			return;
		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			if (Player.player.id != i)
			{
				Global.RoleTeam[i]._Process(delta);
			}
			if (Input.IsKeyPressed((Key)((int)Key.Key1 + i)) && Player.player.id != i && !Global.RoleTeam[i].IsDied)
			{
				Player.player.ChangeRole(Global.RoleTeam[i]);
				Player.player.id = i;
			}
		}
	}

	public void RoleDie()
	{
		_lock = true;
		Player.player._lock = true;
		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			if (!Global.RoleTeam[i].IsDied)
			{
				Player.player.ChangeRole(Global.RoleTeam[i]);
				Player.player.id = i;
				_lock = false;
				Player.player._lock = false;
				break;
			}
		}
		if (_lock)
		{
			GetTree().Paused = true;
			Hud.Instance.GetNode<Label>("Label5").Visible = true;
			_lock = false;
			Player.player._lock = false;
		}

	}
}
