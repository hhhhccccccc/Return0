using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1047 : BattleSkillBase
{
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (ClashState.Contains(false))
        {
            return new BattleSkillRepeatData
            {
                SkillID = GetSkillID(),
                TargetID = Target.EntityID,
                MaxRepeatCount = 999999999,
                IfLostChangeToOther = false
            };
        }

        return null;
    }
}