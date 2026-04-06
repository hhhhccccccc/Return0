using cfg;

public class BattleBuff30361 : BattleBuffBase
{
    protected override int OnGetKeyPropertyMax()
    {
        return LayerCount * GetConfigParamInt(0);
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        DoCheckKeyLimit(Subject);
    }
}
