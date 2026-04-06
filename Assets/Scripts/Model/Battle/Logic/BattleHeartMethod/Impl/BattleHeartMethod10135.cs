using cfg;

public class BattleHeartMethod10135 : BattleHeartMethodBase
{
    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (buffID == GameConst.Battle.BuffDuZhang && layerCount > 0)
        {
            DoAddRandomKey(Subject, GetConfigParamInt(0) * layerCount, ChangeKeyReason.HeartMethodEffect);
            DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(1) * layerCount, BattleSource.HeartMethod);
            DoChangeProperty(Subject, BattlePropertyType.XuanQi, GetConfigParamFloat(2) * layerCount, BattleSource.HeartMethod);
        }
    }
}