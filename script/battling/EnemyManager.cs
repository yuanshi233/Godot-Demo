using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	//刷怪点

	private List<Vector2> pos = [];

	private List<PackedScene> _enemies = [];
	private List<EnemyBase> _pool = [];
	private List<EnemyBase> _activeEnemies = [];

	private Node2D node2D;
	private const float Interval = 2f;
	public override void _Ready()
	{
		node2D = GetNode<Node2D>("../character");
		//test
		pos.Add(new Vector2(500, 10));
		pos.Add(new Vector2(800, 10));
		pos.Add(new Vector2(800, 500));
		pos.Add(new Vector2(-800, 500));
		pos.Add(new Vector2(-500, -500));
		pos.Add(new Vector2(-1000, -500));
		pos.Add(new Vector2(1000, -500));
		pos.Add(new Vector2(500, -500));
		pos.Add(new Vector2(1000, -1000));
		_enemies.Add(GD.Load<PackedScene>("res://scene/character/enemy/Crossbowman/Crossbowman.tscn"));
		Init();
		//test
		Timer timer = new Timer();
		AddChild(timer);
		timer.WaitTime = Interval;
		timer.Timeout += OnTimerTimeout;
		timer.Start();

	}

	private void OnTimerTimeout()
	{
		int lastA = -1;

		foreach (var _ in Enumerable.Range(0, 15))
		{
			int a;
			do
			{
				a = (int)GD.RandRange(0, 8);
			}
			while (a == lastA);

			lastA = a;

			var ene = SpawnEnemy();
			if (ene != null)
			{
				ene.GlobalPosition = pos[a];
				AddChild(ene);
			}
		}


		/*
			GetNode<Timer>("Timer").Stop();
			QueueFree();
		*/

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


	}

	private void Init()
	{
		//敌人最大数量
		for (int i = 0; i < 50; i++)
		{
			var enemy = _enemies[0].Instantiate<EnemyBase>();
			enemy.Visible = false;
			enemy.ProcessMode = ProcessModeEnum.Disabled;
			//简单计算数值
			enemy.HP *= Global.DifficultyCoefficient;
			enemy.AtkVal += Global.DifficultyCoefficient * 10;
			enemy.PhyDef += Global.DifficultyCoefficient;
			enemy.SpellDef += Global.DifficultyCoefficient;

			_pool.Add(enemy);
		}
	}
	private EnemyBase SpawnEnemy()
	{
		if (_pool.Count > 0)
		{
			var enemy = _pool[0];
			_pool.RemoveAt(0);
			_activeEnemies.Add(enemy);

			enemy.Visible = true;
			enemy.ProcessMode = ProcessModeEnum.Inherit;
			enemy.Reset();
			return enemy;
		}
		else
		{
			//GD.Print("pool has no instances!"); 
			return null;
		}
	}
	public void OnEnemyDie(EnemyBase enemy)
	{
		var a = Hud.Instance.KillCount.Text.ToInt() + 1;
		Hud.Instance.KillCount.Text = a.ToString();
		enemy.Visible = false;
		enemy.ProcessMode = ProcessModeEnum.Disabled;
		_activeEnemies.Remove(enemy);
		_pool.Add(enemy);
		RemoveChild(enemy);
		//结算
		if (Hud.Instance.KillCount.Text.ToInt() >= 5)
		{
			
			GetTree().CurrentScene.GetNode<TextureRect>("background").Visible = true;
			GetTree().CurrentScene.GetNode<Godot.Panel>("Panel").Visible = true;
			//转移所有权
			foreach (var i in GetParent().GetNode<Node2D>("character").GetChildren())
			{
				GetParent().GetNode<Node2D>("character").RemoveChild(i);
			}
			GetParent().QueueFree();

			
		}

	}
}
