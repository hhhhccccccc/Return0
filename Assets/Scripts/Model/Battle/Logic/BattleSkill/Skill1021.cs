using System.Collections.Generic;
using Zenject;

public class Skill1021 : BattleSkillBase
{
    // Skill: 观 (1021)
    // XuanQiCost: 20, NeedKey: [4, 2]
    // Moments: DoDesitionMoment [1021001], ReleaseSkillActionMoment [1021002]
    
    // Moment: 1021001 → 无条件 → 自己延迟1息
    public override void DoDesition(MomentParamModel paramModel)
    {
        base.DoDesition(paramModel);
        // 效果: 2900011 - ChangeActionWheel → 自己延迟1息
        // ParamList: [1, -1] → 施法者，延迟1息
        DoChangeActionWheel(Subject, -1);
    }

    // Moment: 1021002 → 无条件 → 自己给自己添加心眼10011,3层 + 自己获得1次行动机会
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111001103 - AddBuff → 自己给自己添加心眼10011,3层
        // ParamList: [1, 1, 10011, 3] → 施法者→自己，10011号Buff，3层
        DoAddBuff(Subject, 10011, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 3400001 - AddActionTimes → 自己获得1次行动机会
        // ParamList: [1, 1] → 施法者，1次
        DoAddActionTimes(Subject, 1);
    }
}