using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3108: BattleSkillBase
{
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        return new BattleSkillRepeatData
        {
            SkillID = SkillID,
            VariantID = VariantID,
            TargetID = Target.EntityID,
            MaxRepeatCount = 2,
            IfLostChangeToOther = false
        };
    }
}