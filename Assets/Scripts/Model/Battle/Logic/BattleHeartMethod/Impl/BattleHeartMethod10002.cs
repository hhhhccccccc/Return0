using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10002 : BattleHeartMethodBase
{
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return 0;
        }

        if (skill.GetSKillType != SkillType.ArtKilling)
        {
            return 0;
        }
        
        if (skill.Config.IsPctCost != 1)
        {
            return 0;
        }
        
        var pct = Subject.GetProperty(BattlePropertyType.XuanQi) / Subject.GetProperty(BattlePropertyType.MaxXuanQi);
        if (pct <= GetConfigParamFloat(0))
        {
            return GetConfigParamFloat(2);
        }

        if (pct >= GetConfigParamFloat(1))
        {
            return GetConfigParamFloat(3);
        }

        var delta = GetConfigParamFloat(1) - GetConfigParamFloat(0);
        delta = (pct - GetConfigParamFloat(0)) / delta;
        var value = (GetConfigParamFloat(3) - GetConfigParamFloat(2)) * delta + GetConfigParamFloat(2);
        return value;
    }
}