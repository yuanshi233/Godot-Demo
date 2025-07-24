using Godot;
using System;
public partial class CardA : Panel
{
    
    [Export] protected Button skill1;
    [Export] protected Button skill2;
    [Export] public String RoleName;

    protected HBoxContainer cardBContainer;
    public CardB cardBInstance;

    public override void _Ready()
    {
        skill1.Toggled += OnSkill1Toggled;
        skill2.Toggled += OnSkill2Toggled;
        cardBContainer = RoleSeleUI.containerB;
        skill2.ButtonPressed = false;
        skill1.ButtonPressed = true;
    }

    private void OnSkill1Toggled(bool toggledOn)
    {
        OnSkillToggled(toggledOn, skill1);
    }
    private void OnSkill2Toggled(bool toggledOn)
    {
        OnSkillToggled(toggledOn, skill2);
    }

    private void OnSkillToggled(bool pressed, Button button)
    {
        if (pressed)
        {
            if (button == skill1)
                skill2.ButtonPressed = false;
            else
                skill1.ButtonPressed = false;
        }
    }
    private void SpawnNewCardB()
    {
        cardBContainer.AddChild(cardBInstance);
        RoleSeleUI.len--;
        GetParent().RemoveChild(this);
    }

    private void OnFreePressed()
    {
        SpawnNewCardB();
    }
}