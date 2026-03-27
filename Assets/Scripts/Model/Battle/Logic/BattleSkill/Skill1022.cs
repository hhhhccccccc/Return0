using System.Collections.Generic;
using Zenject;

public class Skill1022 : BattleSkillBase
{
    // Skill: 灵归 (1022)
    // XuanQiCost: 5000, NeedKey: [2, 1]
    // Moments: DoDesitionMoment [1022002], ReleaseSkillActionMoment [1022003], AfterActionMoment [1022004]
    // ActionDontBeCounter: 1, CheckActionDontBeCounter: [18110221]
    
    // Moment: 1022002 → 无条件 → 我获得5个键
    public override void DoDesition(MomentParamModel paramModel)
    {
        base.DoDesition(paramModel);
        // 效果: 400005 - AddRandomKey → 我获得5个键
        // ParamList: [1, 5, 4] → 施法者，5个键，上限4
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }

    // Moment: 1022003 → 无条件 → 自己移除所有键，添加各种键2个
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4100002 - RemoveAllKeyAndAddAllKey → 自己移除所有键，添加各种键2个
        // ParamList: [1, 2] → 施法者，2个
        DoRemoveAllKeyAndAddAllKey(Subject, 2);
    }

    // Moment: 1022004 → 无条件 → 自己，玄气，75%，招式
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102007 - ChangeProperty → 自己，玄气，75%，招式
        // ParamList: [1, 20054, 0.75, 3] → 施法者，20054(玄气)，0.75(75%)，3(招式)
        DoChangeProperty(Subject, 20054, 0.75f, ChangePropertyReason.Skill);
    }
}