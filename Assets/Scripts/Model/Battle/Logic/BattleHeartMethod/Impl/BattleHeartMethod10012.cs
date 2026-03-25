using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10012 : BattleHeartMethodBase
{
    private int Times => GetParamInt(0);
    private int GangQi => GetParamInt(1);
    private int XuanQi => GetParamInt(2);
    public override void OnKillUnit(int beKillID)
    {
        Subject.AddActionTimes(GetParamInt(0));
        Subject.ChangeProperty(BattlePropertyType.GangQi, GetParamInt(1));
        Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamInt(2));
        
        EnqueueViewModel(Subject.EntityID, MomentViewType.HeartMethod10012, Times, GangQi, XuanQi);
    }
}