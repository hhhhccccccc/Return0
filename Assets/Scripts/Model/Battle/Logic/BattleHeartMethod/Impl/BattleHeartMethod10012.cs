using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10012 : BattleHeartMethodBase
{
    public override void OnKillUnit(int beKillID)
    {
        Subject.AddActionTimes(GetParamInt(0));
        Subject.ChangeProperty(BattlePropertyType.GangQi, GetParamInt(1));
        Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamInt(2));
    }
}