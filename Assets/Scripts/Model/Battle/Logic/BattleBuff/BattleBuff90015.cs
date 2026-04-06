using cfg;

public class BattleBuff90015 : BattleBuffBase
{
    protected override void OnEveryActionWheelStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, Config.ParamEx[0].ToInt(), null, BattleMomentType.EveryActionWheelStart);
    }
}
