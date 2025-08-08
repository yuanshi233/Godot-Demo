using Godot;

public partial class CardC : Node
{
    private ProgressBar HpBar;
    private ProgressBar CdBar;
    public double HP{ set => HpBar.Value = value; }
    public double CD{ set => CdBar.Value = value; }
    public static CardC Instantiate(PackedScene scene, int id)
    {
        CardC instance = scene.Instantiate<CardC>();
        instance.HpBar = instance.GetNode<ProgressBar>("HpBar");
        instance.CdBar = instance.GetNode<ProgressBar>("CdBar");
        instance.GetNode<Label>("id").Text = $"[{id}]";
        return instance;
    }
}
