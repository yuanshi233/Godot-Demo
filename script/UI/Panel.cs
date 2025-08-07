using System;
using Godot;

public partial class Panel : Control
{
    [Export] Label title;
    [Export] Label info;
    public string Title { set => title.Text = value; }
    public string Info { set => info.Text = value; }
    public Action press;
    public override void _Ready()
    {
        GetNode<Button>("Panel/Panel3/Button").ButtonDown += ()=>press?.Invoke();
    }
}
