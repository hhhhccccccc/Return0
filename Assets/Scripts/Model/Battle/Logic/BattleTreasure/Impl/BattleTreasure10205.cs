using cfg;

public class BattleTreasure10205 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        if (Subject.GetAllKeyCount() == GetConfigParamInt(0))
        {
            DoAddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, GetConfigParamInt(1), null, BattleMomentType.RoundStart);
        }
    }
}


