using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10012 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(0);
    private int GangQi => GetConfigParamInt(1);
    private int XuanQi => GetConfigParamInt(2);
    public override void OnKillUnit(int beKillID)
    {
        DoAddActionTimes(Subject, Times);
        DoChangeProperty(Subject, BattlePropertyType.GangQi, GangQi, BattleSource.HeartMethod);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, XuanQi, BattleSource.HeartMethod);
    }
}