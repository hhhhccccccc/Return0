using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10039 : BattleHeartMethodBase
{
    public override void ChangeDamageValue(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.SelfID != Subject.EntityID)
            {
                return;
            }

            var skill = Subject.GetSkill();
            if (skill == null)
            {
                return;
            }

            if (skill.GetSKillType != SkillType.ArtKilling)
            {
                return;
            }

            dict.Add(100000 + Config.Id, GetParamFloat(0) + GetParamFloat(1) * Subject.Gr);
        }
    }
}