using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10039 : BattleHeartMethodBase
{
    private float DamageValue => GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
    public override void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var selfSkillType = model.GetSelfSkillType(Subject.EntityID);
            if (selfSkillType != SkillType.ArtKilling)
            {
                return;
            }

            dict.Add(GetSymbol, DamageValue);
        }
    }
}