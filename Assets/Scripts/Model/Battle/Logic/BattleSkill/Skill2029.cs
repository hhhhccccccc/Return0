using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2029: BattleSkillBase
{
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        return new BattleSkillRepeatData
        {
            SkillID = GetSkillID(),
            TargetID = Target.EntityID,
            MaxRepeatCount = 3,
            IfLostChangeToOther = false
        };
    }
}