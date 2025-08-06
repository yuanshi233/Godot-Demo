using Godot;

public partial class DraggableScrollContainer : ScrollContainer
{
    #region 可配置参数
    [Export(PropertyHint.Range, "0.5,3.0,0.1")]
    public float DragSensitivity { get; set; } = 1.2f;

    [Export(PropertyHint.Range, "5.0,20.0,0.5")] 
    public float Deceleration { get; set; } = 12.0f;
    #endregion

    private bool isDragging;
    private Vector2 startDragPos;
    private Vector2 startScrollPos;
    private Vector2 scrollVelocity;
    private Vector2 lastDragDelta;

    private HScrollBar hBar;
    private VScrollBar vBar;

    public override void _Ready()
    {
        hBar = GetHScrollBar();
        vBar = GetVScrollBar();
    
        MouseFilter = MouseFilterEnum.Pass;
    }

    public override void _Process(double delta)
    {
        float deltaF = (float)delta;
        
        // 惯性滚动
        if (!isDragging && scrollVelocity.Length() > 10f)
        {
            UpdateScrollPosition(scrollVelocity * deltaF);
            scrollVelocity = scrollVelocity.Lerp(Vector2.Zero, Deceleration * deltaF);
        }
    }

    public override void _GuiInput(InputEvent @event)
    {
        // 鼠标开始
        if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
        {
            if (mb.Pressed) StartDrag();
            else EndDrag();
            GetViewport().SetInputAsHandled();
        }
        // 拖动处理
        else if (isDragging && @event is InputEventMouseMotion mm)
        {
            Vector2 delta = -mm.Relative * DragSensitivity;
            lastDragDelta = delta;
            UpdateScrollPosition(delta, applyInertia: false);
            GetViewport().SetInputAsHandled();
        }
    }

    private void StartDrag()
    {
        isDragging = true;
        startDragPos = GetGlobalMousePosition();
        startScrollPos = new Vector2((float)hBar.Value, (float)vBar.Value);
        scrollVelocity = Vector2.Zero;
    }

    private void EndDrag()
    {
        isDragging = false;
        // 计算释放时的惯性速度
        scrollVelocity = lastDragDelta * 15f;
    }

    private void UpdateScrollPosition(Vector2 delta, bool applyInertia = true)
    {
        // 计算新位置
        float newH = (float)(hBar.Value + delta.X);
        float newV = (float)(vBar.Value + delta.Y);

        // 水平边界检查
        if (newH < hBar.MinValue)
        {
            newH = (float)hBar.MinValue;
            if (applyInertia) scrollVelocity.X = 0;
        }
        else if (newH > hBar.MaxValue - hBar.Page)
        {
            newH = (float)Mathf.Max(hBar.MinValue, hBar.MaxValue - hBar.Page);
            if (applyInertia) scrollVelocity.X = 0;
        }

        // 垂直边界检查
        if (newV < vBar.MinValue)
        {
            newV = (float)vBar.MinValue;
            if (applyInertia) scrollVelocity.Y = 0;
        }
        else if (newV > vBar.MaxValue - vBar.Page)
        {
            newV = (float)Mathf.Max(vBar.MinValue, vBar.MaxValue - vBar.Page);
            if (applyInertia) scrollVelocity.Y = 0;
        }

        // 应用新位置
        hBar.Value = newH;
        vBar.Value = newV;
    }
}