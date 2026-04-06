using cfg;

public class BattleTreasure10089 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffNiLin, Subject, GetConfigParamInt(0), null, BattleMomentType.BattleStart);
    }

    protected override void OnRoundEnd()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffNiLin, Subject, GetConfigParamInt(1), null, BattleMomentType.RoundEnd);
    }
}


