using cfg;

public class BattleHeartMethod10140 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffCangShen, Subject, GetConfigParamInt(0), null, BattleMomentType.BattleStart);
        DoAddBuff(Subject, GameConst.Battle.BuffYinHun, Subject, GetConfigParamInt(1), null, BattleMomentType.BattleStart);
    }
}