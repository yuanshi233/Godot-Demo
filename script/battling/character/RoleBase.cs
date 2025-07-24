using System;
using Godot;

public interface RoleBase
{

	public void Attacked(double value, int type);
	public void Attack(int type);
	public void Move(Vector2 inputVector);
	//public void SetPointPos(Vector2 _pos);
	public Vector2 pos{ set; get; }
	public void SetState(Global.State stat);


} 