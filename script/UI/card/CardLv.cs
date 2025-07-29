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
	public int BranchNum { get; set; }
	private float Spacing { get; set; } = 5f;

	public override void _Ready()
	{
		float x0 = Position.X + 125.0f;
		float y0 = Position.Y + BranchNum switch
		{
			0 => 0,
			1 => 120f / 4f,
			2 => 120f / 4f - Spacing / 2f,
			3 => 120f / 4f - Spacing,
			_ => throw new NotImplementedException(),
		};
		List<(float x1, float y1)> pos1 = [];
		List<(float x0, float y0)> pos0 = [];
		switch (BranchNum)
		{
			case 1:
				{
					pos0.Add((x0, y0));
					pos1.Add((x0 + 300f - 125f, y0));
					break;
				}
			case 2:
				{
					pos0.Add((x0, y0));
					pos0.Add((x0, y0 + Spacing));
					pos1.Add((Pos1[0].X, Pos1[0].Y + 30));
					pos1.Add((Pos1[1].X, Pos1[1].Y + 30));
					break;
				}
			case 3:
				{
					pos0.Add((x0, y0 + Spacing));
					pos0.Add((x0, y0));
					pos0.Add((x0, y0+ Spacing * 2));
					pos1.Add((Pos1[0].X, Pos1[0].Y + 30));
					pos1.Add((Pos1[1].X, Pos1[1].Y + 30));
					pos1.Add((Pos1[2].X, Pos1[2].Y + 30));
					break;
				}
			default:
				return;
		}
		for(int i = 0; i < BranchNum; i++)
		{
			ArrowLv arrowLv = line.Instantiate<ArrowLv>();
			arrowLv.Points = GenerateSinusoidalTransition(pos0[i].x0, pos0[i].y0, pos1[i].x1, pos1[i].y1).ToArray();
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
