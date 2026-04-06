using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10015 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(1);
    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round <= GetConfigParamInt(0))
        {
            DoAddActionTimes(Subject, Times);
        }
    }
}