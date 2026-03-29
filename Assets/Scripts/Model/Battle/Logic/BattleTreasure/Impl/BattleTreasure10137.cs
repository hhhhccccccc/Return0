//todo 表现
public class BattleTreasure10137 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, GetParamInt(0));
    }

    protected override void OnRoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, GetParamInt(1));
    }
}