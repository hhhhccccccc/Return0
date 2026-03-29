using cfg;

//todo 表现
public class BattleTreasure10205 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        if (Subject.GetAllKeyCount() == GetParamInt(0))
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, GetParamInt(1));
        }
    }
}


