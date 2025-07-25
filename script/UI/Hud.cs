using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Hud : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public ProgressBar HP_Bar { get; set; }
	[Export]
	public ProgressBar CD_Bar { get; set; }

	public Label KillCount;
	public static Hud Instance;
	public static VBoxContainer RolePane;
	public override void _Ready()
	{
		Instance = this;
		KillCount = GetNode<Label>("Label3");
		RolePane = GetNode<VBoxContainer>("RolePane");
		InitCardC();
	}
	private void InitCardC()
	{
		int i = 1;
		foreach (var r in Global.RoleTeam)
		{
			r.CardCInit(i);
			i++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
