using Godot;

public partial class RoleManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Player player;
	public override void _Ready()
	{
		//重置
		/*
		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			Global.RoleTeam[i].state = new RoleState();
		}
		*/
		player = GetParent().GetNode("Player") as Player;
		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			if (!Global.RoleTeam[i].IsDied)
			{
				player.ChangeRole(Global.RoleTeam[i]);
				player.id = i;
			}
			else if (i == Global.RoleTeam.Count - 1)		//全部阵亡
			{
				GetTree().Paused = true;
				Hud.Instance.GetNode<Label>("Label5").Visible = true;
				_lock = false;
				player._lock = false;
			}
		}
		foreach (var role in Global.RoleTeam)
		{
			role.UpdateCardC();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private bool _lock = false;
	public override void _Process(double delta)
	{
		if (_lock)
			return;
		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			if (player.id != i)
			{
				Global.RoleTeam[i]._Process(delta);
			}
			if (Input.IsKeyPressed((Key)((int)Key.Key1 + i)) && player.id != i && !Global.RoleTeam[i].IsDied)
			{
				player.ChangeRole(Global.RoleTeam[i]);
				player.id = i;
			}
		}
	}

	public void RoleDie()
	{
		_lock = true;
		player._lock = true;
		for (int i = 0; i < Global.RoleTeam.Count; i++)
		{
			if (!Global.RoleTeam[i].IsDied)
			{
				player.ChangeRole(Global.RoleTeam[i]);
				player.id = i;
				_lock = false;
				player._lock = false;
				break;
			}
		}
		if (_lock)
		{
			GetTree().Paused = true;
			Hud.Instance.GetNode<Label>("Label5").Visible = true;
			_lock = false;
			player._lock = false;
		}

	}
}
