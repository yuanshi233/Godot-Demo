using Godot;
using System.Collections.Generic;

public partial class Global : Node
{
    public enum State
    {
        HUD,
        DIE,
        NUL
    }
    public static List<RoleBase> RoleTeam = [];

    public static void ChangeSceneWithStretch(Window root, Window.ContentScaleModeEnum mode, Window.ContentScaleAspectEnum aspect)
    {
        // 设置拉伸模式
        root.ContentScaleMode = mode;
        root.ContentScaleAspect = aspect;
    }

    public static int DifficultyCoefficient = 1;
}
