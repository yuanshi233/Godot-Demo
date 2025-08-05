using Godot;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class ArrowLv : Line2D
{
	private PackedScene _line;
	public Node par { get; set; }
	public PackedScene Line
	{
		get => _line;
		set
		{
			_line = value;
			Init();
		}
	}
	
	private void Init()
	{
		Antialiased = true;
		Width = 5.0f;
		Curve2D curve = new();
		foreach (Vector2 point in this.Points)
		{
			curve.AddPoint(point);
		}
		Points = curve.Tessellate(10);
		/*
		Vector2[] pos = [Points[0], Points[^1]];

		ArrowLv[] arrowLv = [.. Enumerable.Range(0, 2).Select(_ => Line.Instantiate<ArrowLv>())];
		for (int i = 0; i < 2; i++)
		{
			arrowLv[i].Width = 12.0f;
			arrowLv[i].DefaultColor = new Color(255, 255, 255);
			arrowLv[i].Points = [pos[i], new Vector2(pos[i].X + 0.01f, pos[i].Y + 0.01f)];
			par.CallDeferred("add_child", arrowLv[i]);
		}
		*/

	}




	// Called when the node enters the scene tree for the first time.
	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
