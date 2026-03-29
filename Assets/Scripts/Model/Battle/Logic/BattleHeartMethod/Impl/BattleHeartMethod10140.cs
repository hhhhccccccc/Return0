//todo 表现
public class BattleHeartMethod10140 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffCangShen, Subject, GetParamInt(0));
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffYinHun, Subject, GetParamInt(1));
    }
}