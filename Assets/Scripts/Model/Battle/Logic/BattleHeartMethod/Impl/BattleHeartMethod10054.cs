using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10054 : BattleHeartMethodBase
{
    private float DamagePct => GetConfigParamFloat(1);
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return 0;
        }
        
        if (skill.GetKeyCostList.Count >= GetConfigParamInt(0))
        {
            return DamagePct;
        }

        return 0;
    }
}