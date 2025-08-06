using Godot;
using System;

public partial class Bat : Node2D
{
    public override void _Ready()
    {
        Global.ChangeSceneWithStretch(GetTree().Root, Window.ContentScaleModeEnum.Disabled, Window.ContentScaleAspectEnum.Keep);
    }
}
