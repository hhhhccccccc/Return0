using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1022 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        if (CheckLastUseSkillIsBeCounter(Subject, true))
        {
            return 1;
        }
        return 0;
    }
    
    // Skill: 灵归 (1022)
    // XuanQiCost: 5000, NeedKey: [2, 1]
    // Moments: DoDesitionMoment [1022002], ReleaseSkillActionMoment [1022003], AfterActionMoment [1022004]
    // ActionDontBeCounter: 1, CheckActionDontBeCounter: [18110221]
    
    // Moment: 1022002 → 无条件 → 我获得5个键
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 400005 - AddRandomKey → 我获得5个键
        // ParamList: [1, 5, 4] → 施法者，5个键，上限4
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }

    // Moment: 1022003 → 无条件 → 自己移除所有键，添加各种键2个
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //将持有键替换为不同的键各2个
        DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
        DoAddAllKey(Subject, 2, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
    }

    // Moment: 1022004 → 无条件 → 自己，玄气，75%，招式
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102007 - ChangeProperty → 自己，玄气，75%，招式
        // ParamList: [1, 20054, 0.75, 3] → 施法者，20054(玄气)，0.75(75%)，3(招式)
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.75f, BattleSource.Skill);
    }
}