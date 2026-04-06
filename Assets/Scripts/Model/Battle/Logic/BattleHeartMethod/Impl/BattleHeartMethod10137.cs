using cfg;
public class BattleHeartMethod10137 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffYueHuanJiaShi, Subject, GetConfigParamInt(0), null, BattleMomentType.RoundStart);
    }
}