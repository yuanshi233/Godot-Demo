using Godot;
using System;
public partial class CardB : Panel
{
    [Export] public PackedScene CardAScene;
    [Export] public String RoleName;
    private CardA cardAInstance;
    private HBoxContainer cardAContainer;

    public override void _Ready()
    {
        cardAInstance = CardAScene.Instantiate() as CardA;
        cardAInstance.cardBInstance = this;
        cardAContainer = RoleSeleUI.containerA;
    }
    private void OnFreeButton()
    {
        if (RoleSeleUI.len < 4)
        {
            Global.Role r = RoleName switch
            {
                "jesica" => Global.Role.jesica,
                "rosmon" => Global.Role.rosmon,
                _ => throw new ArgumentException("角色名不存在: " + RoleName)
            };
            Global.RoleTeam.Add(r);
            RoleSeleUI.len++;
            cardAContainer.AddChild(cardAInstance as Node);
            GetParent().RemoveChild(this);
        }
    }
    
}