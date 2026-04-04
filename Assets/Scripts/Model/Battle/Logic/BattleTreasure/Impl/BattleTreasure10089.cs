using cfg;

//todo 表现
public class BattleTreasure10089 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffNiLin, Subject, GetConfigParamInt(0));
    }

    protected override void OnRoundEnd()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffNiLin, Subject, GetConfigParamInt(1));
    }
}


