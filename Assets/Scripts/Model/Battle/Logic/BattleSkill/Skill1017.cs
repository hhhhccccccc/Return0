using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1017 : BattleSkillBase
{
    // Moment: 1017001 → 无条件 → 添加玄聚Buff
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111006102 - AddBuff → 自己给自己添加玄聚10061,2层
        DoAddBuff(Subject, 10061, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1017002 → 无条件 → 随机转化键 + 获得行动机会
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 600004 - RandomAllKey → 我随机转化所有+4的键
        // ParamList: [1, 4] → 自己，增加4个键
        DoRandomAllKey(Subject, 4);
        
        // 效果: 3400001 - AddActionTimes → 自己获得1次行动机会
        DoAddActionTimes(Subject, 1);
    }
}