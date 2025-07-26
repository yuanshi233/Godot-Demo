using Godot;
using System;

public partial class Lv : Panel
{
	[Export] public PackedScene _ENCOUNTER;         //不期而遇
	[Export] public PackedScene _COMBAT_OPS;        //作战
	[Export] public PackedScene _SAFE_HOUSE;        //安全的角落
	[Export] public PackedScene _EMERGENCY_OPS;     //紧急作战
	[Export] public PackedScene _THE_ROOT;          //险路恶敌

	public int Extent { set; get; } = 0;

	public void Init()
	{
		
	}
}
