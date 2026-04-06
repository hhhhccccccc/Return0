using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10013 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(1);
    public override void RoundStart()
    {
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <= GetConfigParamFloat(0))
        {
            DoAddActionTimes(Subject, Times);
        }
    }
}