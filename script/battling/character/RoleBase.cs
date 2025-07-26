using System;
using Godot;

public abstract partial class RoleBase : CharacterBody2D
{

	public abstract void Attacked(double value, int type);
	public abstract void Attack(int type);
	public abstract void Move(Vector2 inputVector);
	//public void SetPointPos(Vector2 _pos);
	public abstract Vector2 pos { set; get; }
	public abstract Global.State state { set; get; }
	public PackedScene CardC { get; set; }

	public abstract void CardCInit(int id);

} 