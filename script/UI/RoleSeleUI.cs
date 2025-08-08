using Godot;

public partial class RoleSeleUI : Control
{
	public static RoleSeleUI Instance;

	[Export]
	public HBoxContainer containerB;
	[Export]
	public HBoxContainer containerA;
	

	// Called when the node enters the scene tree for the first time.

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Instance = this;
	}

    public override void _Ready()
    {
		Global.ChangeSceneWithStretch(GetTree().Root, Window.ContentScaleModeEnum.CanvasItems, Window.ContentScaleAspectEnum.Ignore);
    }

	


	private void BackUserUI()
	{
		Global.RoleTeam.Clear();
		var scene = GD.Load<PackedScene>("res://scene/UI/user_ui.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}
	private void OnButtonEnter()
	{
		GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scene/UI/route.tscn"));
	}


}
