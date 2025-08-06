using Godot;
using System;
using System.Collections.Generic;
public partial class CardLv : Control
{
	// 导出的属性，可在Godot编辑器中配置

	/// <summary>
	/// 连接线（箭头）的场景资源
	/// </summary>
	[Export] 
	public PackedScene line { get; set; }
	
	/// <summary>
	/// 关卡的等级/层级
	/// </summary>
	[Export] 
	public int Lv { get; set; }
	
	/// <summary>
	/// 父容器控件，用于添加连接线
	/// </summary>
	public Control Con { get; set; }
	
	/// <summary>
	/// 位置点列表（当前未使用）
	/// </summary>
	public List<Vector2> Pos1 = [];

	/// <summary>
	/// 连接的下一张关卡列表，包含关卡引用和位置索引
	/// </summary>
	public List<(CardLv card, int pos, ArrowLv line)> Nexts = [];
	public void Link(CardLv card, int pos)
	{
		Nexts.Add((card, pos, null));
	}
	/// <summary>
	/// 前置关卡数量（有多少关卡连接到此关卡）
	/// </summary>
	public int PreCount = 0;
	
	/// <summary>
	/// 间距参数（当前未使用）
	/// </summary>
	private float Spacing { get; set; } = 5f;

	float PreY(int row) {
		const float marginy=10,sizeY = 120/2-marginy*2;
		return Position.Y + sizeY / (PreCount+1) * (row+1)+marginy;
	}
	float NextY(int row) {
		const float marginy=10,sizeY = 120/2-marginy*2;
		return Position.Y + sizeY / (Nexts.Count+1) * (row+1)+marginy;
	}

	/// <summary>
	/// 前置关卡连接点的X坐标
	/// </summary>
	float PreX => Position.X;
	
	/// <summary>
	/// 下一张关卡连接点的X坐标
	/// </summary>
	float NextX => Position.X+125;


	/// <summary>
	/// 遮掩节点
	/// </summary> 
	public Panel CoverNode { get; set; }
	
	public void Init()
	{
		CoverNode = GetNode<Panel>("Panel");
		// 为每个下一张关卡创建连接线
		for (int i = 0; i < Nexts.Count; i++)
		{
			// 实例化连接线
			ArrowLv arrowLv = line.Instantiate<ArrowLv>();

			// 生成正弦过渡曲线点集
			// 参数说明：
			// - NextX: 当前关卡连接点的X坐标
			// - NextY[Nexts.Count - 1][i]: 基于连接数量的Y坐标偏移
			// - Nexts[i].card.PreX: 目标关卡连接点的X坐标
			// - PreY[Nexts[i].card.PreCount][Nexts[i].pos]: 基于前置数量的Y坐标偏移

			var next = Nexts[i];
			arrowLv.Points = [.. GenerateSinusoidalTransition(
				NextX,
				NextY(i),
				next.card.PreX,
				next.card.PreY(next.pos)
			)];

			arrowLv.Init();
			next.line = arrowLv;
			Nexts[i] = next;
			Con.AddChild(arrowLv);
			//GetParent().AddChild(arrowLv);
		}
	}

	/// <summary>
	/// 每帧调用的更新方法（当前为空）
	/// </summary>
	/// <param name="delta">自上一帧以来的时间（秒）</param>
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// 生成正弦过渡曲线
	/// </summary>
	/// <param name="x0">起始点X坐标</param>
	/// <param name="y0">起始点Y坐标</param>
	/// <param name="x1">结束点X坐标</param>
	/// <param name="y1">结束点Y坐标</param>
	/// <param name="numPoints">生成的点的数量（默认100）</param>
	/// <returns>包含曲线点坐标的列表</returns>
	public static List<Vector2> GenerateSinusoidalTransition(
		float x0, float y0, float x1, float y1, int numPoints = 30)
	{
		List<Vector2> points = [];
		float deltaX = x1 - x0;

		// 生成numPoints个点
		for (int i = 0; i < numPoints; i++)
		{
			// 计算插值参数t（0到1之间）
			float t = i / (float)(numPoints - 1);
			
			// 线性插值X坐标
			float x = x0 + t * deltaX;
			
			// 使用余弦函数插值Y坐标，创建平滑的S形曲线
			float y = y0 + (y1 - y0) * (0.5f - 0.5f * MathF.Cos(MathF.PI * t));
			
			// 添加点到列表
			points.Add(new Vector2(x, y));
		}
		return points;
	}
}
