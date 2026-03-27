using System.Collections.Generic;
using Zenject;

public class Skill3015 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 6900001 - ReturnSkillResourceCost
        Subject.GetSkill()?.ReturnSkillResourceCost(true, true, true);
    }

}