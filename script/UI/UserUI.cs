using Godot;

public partial class UserUI : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	PackedScene sele_scene;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnButtonDownStart()
	{
		GetTree().ChangeSceneToPacked(sele_scene);
	}
	private void OnQuit()
	{
		GetTree().Quit();
	}
}
