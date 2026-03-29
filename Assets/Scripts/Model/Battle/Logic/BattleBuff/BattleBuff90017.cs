using cfg;

public class BattleBuff90017 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, Config.ParamEx[0].ToInt());
    }

    protected override void OnBuffRemove()
    {
        Subject.ReduceBuffLayerCount(GameConst.Battle.BuffHuiBi, Config.ParamEx[0].ToInt());
    }
}
