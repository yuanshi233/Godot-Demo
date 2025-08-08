using System.Linq;
using Godot;

public partial class Bat : Node2D
{
    public override void _Ready()
    {
        /*
        Global.ChangeSceneWithStretch(GetTree().Root, Window.ContentScaleModeEnum.Disabled, Window.ContentScaleAspectEnum.Keep);
        foreach (Node2D child in GetChildren().OfType<Node2D>())
        {
            child.ZIndex = ZIndex; // 同步Z
        }
            */
    }
}
