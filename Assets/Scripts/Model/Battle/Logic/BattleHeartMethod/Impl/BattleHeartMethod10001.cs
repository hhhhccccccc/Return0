using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10001 : BattleHeartMethodBase
{
    public override float GetDamagePctSum(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return 0;
        }

        if (skill.GetSKillType != SkillType.PowerKilling)
        {
            return 0;
        }

        if (skill.Config.IsPctCost != 1)
        {
            return 0;
        }
        
        var pct = Subject.GetProperty(BattlePropertyType.GangQi) / Subject.GetProperty(BattlePropertyType.MaxGangQi);
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
        var value = (GetParamFloat(3) - GetParamFloat(2)) * delta + GetParamFloat(2);
        EnqueueViewModel(Subject.EntityID, MomentViewType.AddRate, value);
        return value;
    }
}