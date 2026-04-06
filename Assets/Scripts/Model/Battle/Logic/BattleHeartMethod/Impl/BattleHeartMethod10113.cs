using cfg;

public class BattleHeartMethod10113 : BattleHeartMethodBase
{
    private float GangQi => GetConfigParamFloat(0);
    private float XuanQi => GetConfigParamFloat(1);
    private int KeyCount => GetConfigParamInt(2);
    public override void EveryActionWheelStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, GangQi, BattleSource.HeartMethod);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, XuanQi, BattleSource.HeartMethod);
        DoAddRandomKey(Subject, KeyCount, ChangeKeyReason.HeartMethodEffect);
    }
}