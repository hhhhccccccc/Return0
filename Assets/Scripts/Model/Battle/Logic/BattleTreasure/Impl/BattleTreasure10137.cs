//todo 表现
public class BattleTreasure10137 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, GetConfigParamInt(0));
    }

    protected override void OnRoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, GetConfigParamInt(1));
    }
}