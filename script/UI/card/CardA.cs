using Godot;
using System;
using System.Diagnostics;
using System.Dynamic;
public partial class CardA : Panel
{
	
	[Export] protected Button skill1;
	[Export] protected Button skill2;
	[Export] public PackedScene Role;

	public RoleBase RoleInstance{ get; private set; }

	protected HBoxContainer cardBContainer;
	public CardB cardBInstance;
	public override void _Ready()
	{
		skill1.Toggled += OnSkill1Toggled;
		skill2.Toggled += OnSkill2Toggled;
		cardBContainer = RoleSeleUI.Instance.containerB;
		skill2.ButtonPressed = false;
		skill1.ButtonPressed = true;
		RoleInstance = Role.Instantiate<RoleBase>();
	}

	private void OnSkill1Toggled(bool toggledOn)
	{
		if (toggledOn)
			skill2.ButtonPressed = false;
	}
	private void OnSkill2Toggled(bool toggledOn)
	{
		if (toggledOn)
			skill1.ButtonPressed = false;
	}

	private void SpawnNewCardB()
	{
		Global.RoleTeam.Remove(RoleInstance);
		cardBContainer.AddChild(cardBInstance);
		GetParent().RemoveChild(this);
	}

	private void OnFreePressed()
	{
		SpawnNewCardB();
	}
}
