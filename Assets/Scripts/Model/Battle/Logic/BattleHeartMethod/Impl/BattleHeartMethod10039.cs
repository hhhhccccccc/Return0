using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10039 : BattleHeartMethodBase
{
    private float DamageValue => GetParamFloat(0) + GetParamFloat(1) * Subject.Gr;
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
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddDamageInt, GetSymbol, DamageValue);
        }
    }
}