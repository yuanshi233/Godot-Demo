using Godot;
public partial class CardB : Panel
{
	[Export] public PackedScene CardAScene;
	
	private CardA cardAInstance;
	
	public override void _Ready()
	{
		cardAInstance = CardAScene.Instantiate<CardA>();
		cardAInstance.cardBInstance = this;
	}
	private void OnFreeButton()
	{
		if (Global.RoleTeam.Count < 4)
		{
			RoleSeleUI.Instance.containerA.AddChild(cardAInstance);
			Global.RoleTeam.Add(cardAInstance.RoleInstance);
			GetParent().RemoveChild(this);
		}
	}
	
}
