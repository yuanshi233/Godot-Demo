using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Hud : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	ProgressBar HP_Bar { get; set; }
	[Export]
	ProgressBar CD_Bar { get; set; }
	public static double HP{ set => Instance.HP_Bar.Value = value; }
	public static double CD{ set => Instance.CD_Bar.Value = value; }
	public Label KillCount;
	public static Hud Instance;
	public VBoxContainer RolePane;
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
			RolePane.AddChild(r.CardCInit(i));
			i++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
