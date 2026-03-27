using System.Collections.Generic;
using Zenject;

public class Skill1028 : BattleSkillBase
{
    // Skill: 逃之夭夭 (1028)
    // NeedKey: [], CheckSkillDoDesition: [200001], ActionDontBeCounter: 1
    // Moments: ReleaseSkillActionMoment [1028002]
    
    // Moment: 1028002 → 无条件 → TODO: 自己退出战斗
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // TODO: 效果: 9800001 - ExitBattle → 自己退出战斗
        // 这个效果需要在 BattleSkillBase 中添加封装方法
    }
}