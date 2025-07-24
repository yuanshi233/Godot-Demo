using Godot;
using System;

public partial class RoleSeleUI : Control
{

	// Called when the node enters the scene tree for the first time.
	public static int len = 0;
	public override void _Ready()
	{
		containerB = GetNode<HBoxContainer>("Panel/ScrollContainer/HBoxContainer"); 
		containerA = GetNode<HBoxContainer>("PanelContainer/HBoxContainer");
		for (int i = 0; i < 5; i++)
		{
			PackedScene card = GD.Load<PackedScene>("res://scene/character/role/jesica/card/jesica_card_B.tscn");
			GetNode<HBoxContainer>("Panel/ScrollContainer/HBoxContainer").AddChild(card.Instantiate());
			card = GD.Load<PackedScene>("res://scene/character/role/rosmon/card/rosmon_card_B.tscn");
			GetNode<HBoxContainer>("Panel/ScrollContainer/HBoxContainer").AddChild(card.Instantiate());
		}


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void BackUserUI()
	{
		RoleSeleUI.len = 0;
		var scene = GD.Load<PackedScene>("res://scene/UI/user_ui.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}
	private void OnButtonEnter()
	{
		
	}

	public static HBoxContainer containerB;
	public static HBoxContainer containerA;

}
