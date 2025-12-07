using cfg;

public class BattleBuff30361 : BattleBuffBase
{
    protected override int OnGetKeyPropertyMax()
    {
        return LayerCount * Config.ParamEx[0].ToInt();
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        Subject.CheckKeyLimit();
    }
}
