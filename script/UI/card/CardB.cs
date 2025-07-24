using Godot;
using System;
using System.Diagnostics;
public partial class CardB : Panel
{
	[Export] public PackedScene CardAScene;
	
	private CardA cardAInstance;
	
	public override void _Ready()
	{
		cardAInstance = CardAScene.Instantiate() as CardA;
		cardAInstance.cardBInstance = this;
	}
	private void OnFreeButton()
	{
		if (RoleSeleUI.Instance.len < 4)
		{
			RoleSeleUI.Instance.len++;
			RoleSeleUI.Instance.containerA.AddChild(cardAInstance);
			GetParent().RemoveChild(this);
		}
	}
	
}
