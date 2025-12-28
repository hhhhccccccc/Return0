using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10002 : BattleHeartMethodBase
{
    public override float GetSkillDamageRate(MomentParamModel paramModel)
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
        if (pct <= GetParamFloat(0))
        {
            return GetParamFloat(2);
        }

        if (pct >= GetParamFloat(1))
        {
            return GetParamFloat(3);
        }

        var delta = GetParamFloat(1) - GetParamFloat(0);
        delta = (pct - GetParamFloat(0)) / delta;
        return (GetParamFloat(3) - GetParamFloat(2)) * delta + GetParamFloat(2);
    }
}