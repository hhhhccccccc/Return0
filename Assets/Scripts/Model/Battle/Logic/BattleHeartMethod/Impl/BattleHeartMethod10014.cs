using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10014 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round > GetParamInt(0))
        {
            Subject.AddActionTimes(GetParamInt(1));
        }
    }
}