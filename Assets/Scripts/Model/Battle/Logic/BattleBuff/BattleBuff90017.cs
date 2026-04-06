using cfg;

public class BattleBuff90017 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, GetConfigParamInt(0), null, BattleMomentType.None);
    }

    protected override void OnBuffRemove()
    {
        DoReduceBuffLayerCount(Subject, GameConst.Battle.BuffHuiBi, GetConfigParamInt(0));
    }
}
