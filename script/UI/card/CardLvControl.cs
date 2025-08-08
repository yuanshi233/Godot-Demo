using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CardLvControl : Button
{
    [Export] public int Lv { get; set; }
    [Export] string title = "[标题]";
    [Export] string info = "[关卡信息INFO]";
    private PackedScene bat;
    private bool isMouseOver = false;
    Panel panel;
    public override void _Ready()
    {
        bat = GD.Load<PackedScene>("res://scene/UI/bat.tscn");
        ButtonDown += OnButtonDown;
        panel = (FindParent("Route") as Route).panel;
    }

    public override void _Process(double delta)
    {
        CheckHoverStatus();
    }

    private void OnButtonDown()
    {
        if (panel.GetParent() != this)
        {
            panel.GetParent()?.RemoveChild(panel);
            panel.Title = title;
            panel.Info = info;
            Button b = panel.GetNode<Button>("Panel/Panel3/Button");
            panel.press = EnterAndChangeCow;
            AddChild(panel);
            GetParent().GetParent().MoveChild(GetParent(), -1);
            //FindParent("Control").MoveChild(GetParent(), -1);
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

    public static int currentCowIndex = 0;
    private Bat batInst;
    private void EnterAndChangeCow()
    {
        if (GetTree().CurrentScene is not Route route || GetParent() is not CardLv currentCard)
        {
            return;
        }

        // 检查边界条件
        if (currentCowIndex > route.cardLv.Length)
        {
            return;
        }

        //加入战斗
        Global.DifficultyCoefficient = Lv;
        batInst = bat.Instantiate<Bat>();
        GetTree().CurrentScene.GetNode<TextureRect>("background").Visible = false;
		GetTree().CurrentScene.GetNode<Godot.Panel>("Panel").Visible = false;
        GetTree().CurrentScene.AddChild(batInst);

        // 处理当前列
        ProcessCardRow(route.cardLv[currentCowIndex], true, Color.Color8(45, 45, 45));
        panel.GetParent()?.RemoveChild(panel);

        // 处理下一列
        var nextCards = currentCard.Nexts.Select(next => next.card).ToArray();
        ProcessCardRow(nextCards, false, Color.Color8(145, 145, 145));

        currentCowIndex++;
    }

    public static void ProcessCardRow(IEnumerable<CardLv> cards, bool coverVisible, Color lineColor)
    {
        foreach (var card in cards)
        {
            card.CoverNode.Visible = coverVisible;
            card.GetNode<Button>("Button").MouseFilter = coverVisible ? MouseFilterEnum.Ignore : MouseFilterEnum.Pass;
            foreach (var next in card.Nexts)
            {
                next.line.DefaultColor = lineColor;
            }
        }
    }

    private void CheckHoverStatus()
    {
        if (panel.GetParent() != this)
            return;
        Vector2 mousePos = GetGlobalMousePosition();
        Rect2 thisRect = new(GlobalPosition, Size / 2);
        Rect2 panelRect = new(panel.GlobalPosition, panel.Size / 2);
        if (!thisRect.Merge(panelRect).HasPoint(mousePos))
        {
            RemoveChild(panel);
        }
    }
}
