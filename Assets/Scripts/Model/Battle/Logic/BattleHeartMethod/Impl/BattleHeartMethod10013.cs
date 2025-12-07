using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10013 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <= GetParamFloat(0))
        {
            Subject.AddActionTimes(GetParamInt(1));
        }
    }
}