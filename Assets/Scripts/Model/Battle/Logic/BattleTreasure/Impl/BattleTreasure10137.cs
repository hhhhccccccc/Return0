using cfg;

public class BattleTreasure10137 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, GetConfigParamInt(0), null, BattleMomentType.BattleStart);
    }

    protected override void OnRoundStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, GetConfigParamInt(1), null, BattleMomentType.RoundStart);
    }
}