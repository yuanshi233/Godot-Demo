using Godot;
using System;
using System.Diagnostics;
public partial class CardB : Panel
{
	[Export] public PackedScene CardAScene;
	[Export] public PackedScene Role;
	private CardA cardAInstance;
	RoleBase RoleInstance;
	public override void _Ready()
	{
		cardAInstance = CardAScene.Instantiate() as CardA;
		cardAInstance.cardBInstance = this;
		RoleInstance = Role.Instantiate<RoleBase>();
	}
	private void OnFreeButton()
	{
		if (RoleSeleUI.Instance.len < 4)
		{
			Global.RoleTeam.Add(RoleInstance);
			RoleSeleUI.Instance.len++;
			RoleSeleUI.Instance.containerA.AddChild(cardAInstance);
			GetParent().RemoveChild(this);
		}
	}
	
}
