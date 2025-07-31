using Godot;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;

public partial class Route : Control
{
	[Export] public Godot.Collections.Dictionary<PackedScene, float> CardLvs;
	[Export] public Godot.Collections.Dictionary<int, float> RowCountProbably;
	[Export] public Godot.Collections.Dictionary<int, float> CowCountProbably;
	[Export] public Godot.Collections.Array<float> cardx;
	[Export] public Godot.Collections.Dictionary<int, Godot.Collections.Array<float>> cardy;

	//探索级别
	public int MainLv;

	private CardLv[][] cardLv;
	public override void _Ready()
	{
		cardLv = new CardLv[WeightedRandom(CowCountProbably)][];
		for (int i = 0; i < cardLv.Length; i++)
		{
			int rowCount = WeightedRandom(RowCountProbably);
			cardLv[i] = new CardLv[rowCount];
			for (int j = 0; j < cardLv[i].Length; j++)
			{
				cardLv[i][j] = WeightedRandom(CardLvs).Instantiate<CardLv>();
				cardLv[i][j].Position = new Vector2(cardx[i], cardy[i][j]);
				cardLv[i][j].Con = GetNode<Control>("Panel/ScrollContainer/Control");
			}
			if (i > 0)
			{
				if (cardLv[i - 1].Length == 1 && rowCount == 1)
				{
					cardLv[i - 1][0].Nexts.Add((cardLv[i][0], 0));
					cardLv[i][0].PreCount++;
					continue;
				}
				Stack<(int, int, int, int)> s = new();
				s.Push((0, cardLv[i - 1].Length - 1, 0, rowCount - 1));
				List<int>[] lines = new List<int>[cardLv[i - 1].Length];
				List<int>[] revlines = new List<int>[cardLv[i].Length];
				for (int j = 0; j < lines.Length; j++)
				{
					lines[j] = [];
				}
				for (int j = 0; j < revlines.Length; j++)
				{
					revlines[j] = [];
				}
				lines[0].Add(0);
				revlines[0].Add(0);
				lines[cardLv[i - 1].Length - 1].Add(rowCount - 1);
				revlines[cardLv[i - 1].Length - 1].Add(rowCount - 1);
				while (s.Count > 0)
				{
					var (ff, fl, tf, tl) = s.Pop();
					int f = random.Next(ff, fl + 1);
					if (ff == f)
					{
						int t = random.Next(tf + 1, tl + 1);
						for (int j = tf + 1; j <= t; j++)
						{
							lines[f].Add(j);
							revlines[j].Add(f);
						}
						s.Push((f, fl, t, tl));
					}
					else if (f == fl)
					{
						int t = random.Next(tf, tl);
						for (int j = t; j < tl; j++)
						{
							lines[f].Add(j);
							revlines[j].Add(f);
						}
						s.Push((ff, f, tf, t));
					}
					else
					{
						int t = random.Next(tf, tl + 1);
						if (tf == t)
						{
							for (int j = ff + 1; j <= f; j++)
							{
								lines[j].Add(t);
								revlines[t].Add(j);
							}
							s.Push((f, fl, t, tl));
						}
						else if (f == fl)
						{
							for (int j = f; j < fl; j++)
							{
								lines[j].Add(t);
								revlines[t].Add(j);
							}
							s.Push((ff, f, tf, t));
						}
						else
						{
							lines[f].Add(t);
							revlines[t].Add(f);
							s.Push((f, fl, t, tl));
							s.Push((ff, f, tf, t));
						}
					}
				}
				for (int j = 0; j < lines.Length; j++)
				{
					lines[j].Sort();
				}
				for (int j = 0; j < revlines.Length; j++)
				{
					revlines[j].Sort();;
				}
				for (int j = 0; j < lines.Length; j++)
				{
					foreach (int k in lines[j])
					{
						cardLv[i - 1][j].Nexts.Add((cardLv[i][k], revlines[k].IndexOf(j)));
						cardLv[i][j].PreCount++;
					}
				}
			}
		}
		Control b = new()
		{
			CustomMinimumSize = new Vector2(5 * 300 + 100, 0)
		};
		GetNode<ScrollContainer>("Panel/ScrollContainer").AddChild(b);

		//加载
		var c = GetNode<Control>("Panel/ScrollContainer/Control");
		foreach (var i in cardLv)
			foreach (var j in i)
				c.AddChild(j);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	//权重随机
	private static readonly Random random = new();
	public static T WeightedRandom<T>(IEnumerable<(T, float)> weightedItems)
	{
		float totalWeight = 0;
		foreach (var pair in weightedItems)
			totalWeight += pair.Item2;

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
	public static T WeightedRandom<T>(IEnumerable<KeyValuePair<T, float>> weightedItems)
	{
		float totalWeight = 0;
		foreach (var pair in weightedItems)
			totalWeight += pair.Value;

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
