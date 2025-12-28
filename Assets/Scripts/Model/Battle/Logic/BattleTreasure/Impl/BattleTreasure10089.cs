using cfg;

public class BattleTreasure10089 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30041, Subject, GetParamInt(0));
    }

    protected override void OnRoundEnd()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30041, Subject, GetParamInt(1));
    }
}


