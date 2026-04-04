using cfg;

//todo 表现
public class BattleTreasure10205 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        if (Subject.GetAllKeyCount() == GetConfigParamInt(0))
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, GetConfigParamInt(1));
        }
    }
}


