using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3103: BattleSkillBase
{
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        return new BattleSkillRepeatData
        {
            SkillID = GetSkillID(),
            TargetID = Target.EntityID,
            MaxRepeatCount = 2,
            IfLostChangeToOther = false
        };
    }
}