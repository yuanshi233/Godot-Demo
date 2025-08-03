using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

public partial class Route : Control
{
	// 导出的属性，可在Godot编辑器中配置

	/// <summary>
	/// 关卡类型及其生成概率（PackedScene是场景资源，float是权重概率）
	/// </summary>
	[Export]
	public Godot.Collections.Dictionary<PackedScene, float> CardLvs;

	/// <summary>
	/// 行数的生成概率（int是行数，float是权重概率）
	/// </summary>
	[Export]
	public Godot.Collections.Dictionary<int, float> RowCountProbably = [];

	/// <summary>
	/// 列数的生成概率（int是列数，float是权重概率）
	/// </summary>
	[Export]
	public Godot.Collections.Dictionary<int, float> CowCountProbably = [];

	/// <summary>
	/// 每列关卡的X轴位置
	/// </summary>
	//[Export] 
	public static float Cardx(int col)
	{
		const float sizex = 250 / 2, spacex = 100 / 2;
		return col * (sizex + spacex);
	}

	/// <summary>
	/// 每列关卡的Y轴位置（字典键是列索引，数组是每行的Y坐标）
	/// </summary>
	//[Export] 
	public static float Cardy(int rowCount, int row)
	{
		const float sizey = 120 / 2, spacey = 60 / 2;
		return row * (sizey + spacey);
	}

	/// <summary>
	/// 探索级别/主关卡级别
	/// </summary>
	public int MainLv;

	/// <summary>
	/// 存储所有关卡的二维数组 [列][行]
	/// </summary>
	private CardLv[][] cardLv;

	// 随机数生成器
	private static readonly Random random = new();

	public override void _Ready()
	{

		// 1. 首先确定要生成多少列（基于CowCountProbably的概率权重）
		cardLv = new CardLv[WeightedRandom(CowCountProbably)][];

		// 遍历每一列
		for (int i = 0; i < cardLv.Length; i++)
		{
			// 2. 确定当前列有多少行（基于RowCountProbably的概率权重）
			int rowCount = WeightedRandom(RowCountProbably);
			cardLv[i] = new CardLv[rowCount];

			// 3. 生成当前列的每一行关卡
			for (int j = 0; j < cardLv[i].Length; j++)
			{
				// 3.1 随机选择关卡类型并实例化
				cardLv[i][j] = WeightedRandom(CardLvs).Instantiate<CardLv>();
				// 3.2 设置关卡位置
				cardLv[i][j].Position = new Vector2(Cardx(i), Cardy(rowCount - 1, j));
				// 3.3 设置关卡的父容器
				cardLv[i][j].Con = GetNode<Control>("Panel/ScrollContainer/Control");
			}
		}
		Link();
		// 8. 创建足够大的容器以容纳生成的路径
		Control b = new()
		{
			CustomMinimumSize = new Vector2(5 * 300 + 100, 0) // 宽度计算，留有余量
		};
		GetNode<ScrollContainer>("Panel/ScrollContainer").AddChild(b);

		// 9. 将所有关卡添加到场景中
		var c = GetNode<Control>("Panel/ScrollContainer/Control");
		foreach (var column in cardLv)
		{
			foreach (var card in column)
			{
				c.AddChild(card);
			}
		}
	}

	void Link()
	{
		// 遍历每一列
		for (int i = 1; i < cardLv.Length; i++)
		{
			// 2. 确定当前列有多少行（基于RowCountProbably的概率权重）
			int rowCount = cardLv[i].Length;

			// 4. 特殊情况处理：前后列都只有1个节点，直接连接
			if (cardLv[i - 1].Length == 1 && rowCount == 1)
			{
				cardLv[i - 1][0].Nexts.Add((cardLv[i][0], 0));
				cardLv[i][0].PreCount++;
				continue;
			}

			// 5. 通用连接算法：使用分治法生成节点连接关系

			// 使用堆栈存储待处理的分区（前列起始/结束行，当前列起始/结束行）
			Stack<(int, int, int, int)> s = new();
			s.Push((0, cardLv[i - 1].Length - 1, 0, rowCount - 1));

			// 存储连接关系：
			// lines[前排行] = 连接的当前行列表
			// revlines[当前行] = 连接的前排行列表
			List<int>[] lines = new List<int>[cardLv[i - 1].Length];
			List<int>[] revlines = new List<int>[cardLv[i].Length];

			// 初始化连接列表
			for (int j = 0; j < lines.Length; j++) lines[j] = new List<int>();
			for (int j = 0; j < revlines.Length; j++) revlines[j] = new List<int>();

			// 确保首尾连接（最上行连接最上行，最下行连接最下行）
			lines[0].Add(0);
			revlines[0].Add(0);
			lines[cardLv[i - 1].Length - 1].Add(rowCount - 1);
			revlines[rowCount - 1].Add(cardLv[i - 1].Length - 1);

			// 分治处理所有分区
			while (s.Count > 0)
			{
				var (ff, fl, tf, tl) = s.Pop(); // 前排行范围(ff-fl)，当前行范围(tf-tl)
												// 随机选择前排行分割点
				int f = random.Next(ff, fl + 1);
				if (ff == f) // 分割点在分区最左
				{
					// 随机选择当前行分割点（必须大于tf）
					int t = random.Next(tf + 1, tl + 1);
					// 连接f行到tf+1至t行
					for (int j = tf + 1; j <= t; j++)
					{
						lines[f].Add(j);
						revlines[j].Add(f);
					}
					if (t < tl) s.Push((f, fl, t, tl));
					else for (int j = ff + 1; j < fl; j++)
						{
							lines[j].Add(t);
							revlines[t].Add(j);
						}
				}
				else if (f == fl) // 分割点在分区最右
				{
					// 随机选择当前行分割点（必须小于tl）
					int t = random.Next(tf, tl);
					// 连接f行到t至tl-1行
					for (int j = t; j < tl; j++)
					{
						lines[f].Add(j);
						revlines[j].Add(f);
					}
					// 将左侧新区间压栈
					if (t > tf) s.Push((ff, f, tf, t));
					else for (int j = ff + 1; j < fl; j++)
						{
							lines[j].Add(t);
							revlines[t].Add(j);
						}
				}
				else // 分割点在分区中间
				{
					// 随机选择当前行分割点
					int t = random.Next(tf, tl + 1);

					if (tf == t) // 当前行分割点在最左
					{
						// 连接ff+1至f行到t行
						for (int j = ff + 1; j <= f; j++)
						{
							lines[j].Add(t);
							revlines[t].Add(j);
						}
						// 处理右侧区间
						s.Push((f, fl, t, tl));
					}
					else if (t == tl) // 当前行分割点在最右
					{
						// 连接f至fl-1行到t行
						for (int j = f; j < fl; j++)
						{
							lines[j].Add(t);
							revlines[t].Add(j);
						}
						// 处理左侧区间
						s.Push((ff, f, tf, t));
					}
					else // 普通情况
					{
						// 连接分割点
						lines[f].Add(t);
						revlines[t].Add(f);
						// 分别处理左右区间
						s.Push((f, fl, t, tl));
						s.Push((ff, f, tf, t));
					}
				}
			}

			// 6. 对连接关系进行排序
			for (int j = 0; j < lines.Length; j++) lines[j].Sort();
			for (int j = 0; j < revlines.Length; j++) revlines[j].Sort();
			// 7. 实际建立关卡之间的连接关系
			for (int j = 0; j < lines.Length; j++)
			{
				foreach (int k in lines[j])
				{
					// 添加到前一关卡的Nexts列表
					cardLv[i - 1][j].Nexts.Add((cardLv[i][k], revlines[k].IndexOf(j)));
					// 增加后一关卡的PreCount（前置条件计数）
					cardLv[i][k].PreCount++;
				}
			}
		}
	}
	/// <summary>
	/// 权重随机方法（元组版本）
	/// </summary>
	/// <param name="weightedItems">带权重的选项集合</param>
	/// <returns>随机选中的项</returns>
	public static T WeightedRandom<T>(IEnumerable<(T, float)> weightedItems)
	{
		// 计算总权重
		float totalWeight = 0;
		foreach (var pair in weightedItems)
			totalWeight += pair.Item2;

		// 随机选择
		float randomValue = (float)random.NextDouble() * totalWeight;
		float currentSum = 0;
		foreach (var pair in weightedItems)
		{
			currentSum += pair.Item2;
			if (randomValue <= currentSum)
				return pair.Item1;
		}
		return default;
	}

	/// <summary>
	/// 权重随机方法（键值对版本）
	/// </summary>
	/// <param name="weightedItems">带权重的选项字典</param>
	/// <returns>随机选中的项</returns>
	public static T WeightedRandom<T>(IEnumerable<KeyValuePair<T, float>> weightedItems)
	{
		// 计算总权重
		float totalWeight = 0;
		foreach (var pair in weightedItems)
			totalWeight += pair.Value;

		// 随机选择
		float randomValue = (float)random.NextDouble() * totalWeight;
		float currentSum = 0;
		foreach (var pair in weightedItems)
		{
			currentSum += pair.Value;
			if (randomValue <= currentSum)
				return pair.Key;
		}
		return default;
	}
}
