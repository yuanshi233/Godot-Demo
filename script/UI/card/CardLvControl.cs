using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CardLvControl : Button
{
    [Export] public int Lv { get; set; }
    [Export] public PackedScene LvPanelScene { get; set; }

    private Control panel;
    private bool isMouseOver = false;

    public override void _Ready()
    {
        ButtonDown += OnButtonDown;
    }

    public override void _Process(double delta)
    {
        CheckHoverStatus();
    }

    private void OnButtonDown()
    {
        // 如果panel不存在或已被释放，则实例化
        if (!IsInstanceValid(panel))
        {
            panel = LvPanelScene.Instantiate<Control>();
            panel.GetNode<Button>("Panel/Panel3/Button").ButtonDown += EnterAndChangeCow;
            AddChild(panel);
        }
        // 设置位置
        if (GetViewportRect().Size.Y < GlobalPosition.Y + panel.Size.Y / 2)
        {
            panel.Position = new Vector2(Position.X + 250, Position.Y + GetViewportRect().Size.Y - GlobalPosition.Y - panel.Size.Y);
        }
        else
        {
            panel.Position = new Vector2(Position.X + 250, Position.Y);
        }

    }

    private static int currentCowIndex = 0; 

    private void EnterAndChangeCow()
    {
        if (GetTree().CurrentScene is not Route route || GetParent() is not CardLv currentCard)
        {
            return;
        }

        // 检查边界条件
        if (currentCowIndex >= route.cardLv.Length)
        {
            return;
        }

        // 处理当前列
        ProcessCardRow(route.cardLv[currentCowIndex], true, Color.Color8(45, 45, 45));
        panel.QueueFree();
        panel = null;

        // 处理下一列
        var nextCards = currentCard.Nexts.Select(next => next.card).ToArray();
        ProcessCardRow(nextCards, false, Color.Color8(145, 145, 145));

        currentCowIndex++;
    }

    private void ProcessCardRow(IEnumerable<CardLv> cards, bool coverVisible, Color lineColor)
    {
        foreach (var card in cards)
        {
            card.CoverNode.Visible = coverVisible;

            foreach (var next in card.Nexts)
            {
                next.line.DefaultColor = lineColor;
            }
        }
    }

    private void CheckHoverStatus()
    {
        if (!IsInstanceValid(panel))
            return;
        Vector2 mousePos = GetGlobalMousePosition();
        Rect2 thisRect = new(GlobalPosition, Size / 2);
        Rect2 panelRect = new(panel.GlobalPosition, panel.Size / 2);
        if (!thisRect.HasPoint(mousePos) && !panelRect.HasPoint(mousePos))
        {
            panel.QueueFree();
            panel = null;
        }
    }
}