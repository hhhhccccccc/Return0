using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10102 : BattleHeartMethodBase
{
    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (pType == BattlePropertyType.Hp && source == BattleSource.Skill)
        {
            value += GetProperty(BattlePropertyType.Tech) * GetConfigParamFloat(0);
        }
    }
}