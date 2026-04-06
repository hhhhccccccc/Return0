//todo 表现

using cfg;

public class BattleHeartMethod10068 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffFuXiaoJian, Subject, GetConfigParamInt(0), null, BattleMomentType.BattleStart);
    }
}