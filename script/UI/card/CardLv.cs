using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

public partial class CardLv : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene line { get; set; }
	[Export] public int Lv { get; set; }
	public Control Con { get; set; }
	public List<Vector2> Pos1 = [];
	public List<(CardLv card,int pos)> Nexts=[];
	public int PreCount=0;
	private float Spacing { get; set; } = 5f;
	static float[][] PreY=new Func<float[][]>(()=>
	{
		throw new NotImplementedException();
	})(), NextY=new Func<float[][]>(()=>
	{
		throw new NotImplementedException();
	})();
	float PreX => throw new NotImplementedException();
	float NextX=>throw new NotImplementedException();
	public override void _Ready()
	{
		for (int i = 0; i < Nexts.Count; i++)
		{
			ArrowLv arrowLv = line.Instantiate<ArrowLv>();
			arrowLv.Points = GenerateSinusoidalTransition(NextX,NextY[Nexts.Count - 1][i], Nexts[i].card.PreX, PreY[Nexts[i].card.PreCount][Nexts[i].pos]).ToArray();
			Con.AddChild(arrowLv);
		}


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public static List<Vector2> GenerateSinusoidalTransition(
		float x0, float y0, float x1, float y1, int numPoints = 100)
	{
		List<Vector2> points = [];
		float deltaX = x1 - x0;

		for (int i = 0; i < numPoints; i++)
		{
			float t = i / (float)(numPoints - 1);
			float x = x0 + t * deltaX;
			float y = y0 + (y1 - y0) * (0.5f - 0.5f * MathF.Cos(MathF.PI * t));
			points.Add(new Vector2(x, y));
		}
		return points;
	}




}
