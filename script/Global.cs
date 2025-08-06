using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

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
}
