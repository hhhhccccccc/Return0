//todo 表现
public class BattleHeartMethod10140 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffCangShen, Subject, GetConfigParamInt(0));
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffYinHun, Subject, GetConfigParamInt(1));
    }
}