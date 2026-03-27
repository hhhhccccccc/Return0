using System.Collections.Generic;
using Zenject;

public class Skill1020 : BattleSkillBase
{
    // Moment: 1020001 → 无条件 → 添加随机键
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 400005 - AddRandomKey → 我获得5个键
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }

    // Moment: 1020002 → 无条件 → 移除所有键，添加各种键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4100002 - RemoveAllKeyAndAddAllKey → 自己移除所有键，添加各种键2个
        // ParamList: [1, 2] → 自己，每种键2个
        DoRemoveAllKeyAndAddAllKey(Subject, 2);
    }

    // Moment: 1020003 → 无条件 → 玄气百分比变化
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102007 - ChangeProperty → 自己，玄气，75%，招式
        // ParamList: [1, 20054, 0.75, 3] → 自己，20054(XuanQiPct)，0.75，来源3(招式)
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.75f, BattleSource.None);
    }
}