//todo 表现
public class BattleHeartMethod10137 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffYueHuanJiaShi, Subject, GetConfigParamInt(0));
    }
}