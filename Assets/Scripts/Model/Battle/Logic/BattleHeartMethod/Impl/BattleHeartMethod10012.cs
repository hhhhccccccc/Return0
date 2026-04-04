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
        Subject.AddActionTimes(GetConfigParamInt(0));
        Subject.ChangeProperty(BattlePropertyType.GangQi, GetConfigParamInt(1));
        Subject.ChangeProperty(BattlePropertyType.XuanQi, GetConfigParamInt(2));
        
        EnqueueViewModel(Subject.EntityID, MomentViewType.HeartMethod10012, Times, GangQi, XuanQi);
    }
}