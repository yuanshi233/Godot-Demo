using Godot;

using System;
using System.Collections.Generic;

public partial class Route : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene CardLv1;
	[Export] public PackedScene CardLv2;
	[Export] public PackedScene CardLv3;

	//探索级别
	public int MainLv;
	//层数
	public int LayerNum;
	//
	private SortedDictionary<int, float> lootTableLv = [];
	private SortedDictionary<int, float> lootTableBranch = [];

	private CardLv[,] cardLv = new CardLv[3, 5];

	public override void _Ready()
	{
		//关卡难度 1 2 3
		lootTableLv.Add(1, 60.0f);
		lootTableLv.Add(2, 30.0f);
		lootTableLv.Add(3, 10.0f);

		//分支线
		lootTableBranch.Add(1, 60.0f);
		lootTableBranch.Add(2, 30.0f);
		lootTableBranch.Add(3, 10.0f);

		for (int i = 0; i < 3; i++)
		{
			for (int a = 0; a < 5; a++)
			{
				cardLv[i, a] = WeightedRandom(lootTableLv) switch
				{
					1 => CardLv1.Instantiate<CardLv>(),
					2 => CardLv2.Instantiate<CardLv>(),
					3 => CardLv3.Instantiate<CardLv>(),
					_ => throw new NotImplementedException()
				};
				cardLv[i, a].BranchNum = WeightedRandom(lootTableBranch);
				cardLv[i, a].Position = new Vector2(a * 300 + 100, i * 150 + 100);
				cardLv[i, a].Con = GetNode<Control>("Panel/ScrollContainer/Control");
			}
		}
		for (int i = 0; i < 3; i++)
		{
			for (int d = 0; d < 4; d++)
			{
				cardLv[i, d].Pos1.Add(cardLv[i, d + 1].Position);
				if(i != 0) 
					cardLv[i, d].Pos1.Add(cardLv[0, d + 1].Position);
				if(i != 1)
					cardLv[i, d].Pos1.Add(cardLv[1, d + 1].Position);
				if(i != 2)
					cardLv[i, d].Pos1.Add(cardLv[2, d + 1].Position);
			}
		}
		//确保最后一个没有路线
		cardLv[0, 4].BranchNum = 0;
		cardLv[1, 4].BranchNum = 0;
		cardLv[2, 4].BranchNum = 0;

        Control b = new()
        {
            CustomMinimumSize = new Vector2(5 * 300 + 100, 0)
        };
        GetNode<ScrollContainer>("Panel/ScrollContainer").AddChild(b);

		//加载
		var c = GetNode<Control>("Panel/ScrollContainer/Control");
		foreach (var i in cardLv)
			c.AddChild(i);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	//权重随机
	private static readonly Random random = new();
	public static T WeightedRandom<T>(IReadOnlyDictionary<T, float> weightedItems)
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
