using System.Collections.Generic;
using Zenject;

public class Skill1018 : BattleSkillBase
{
    // Moment: 1018002 → 无条件 → 玄气百分比上限变化
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 104001 - ChangeProperty → 自己，玄气百分比上限，0.4，招式
        // ParamList: [1, 20044, 0.4, 3] → 自己，20044(MaxXuanQiPct)，0.4，来源3(招式)
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiPct, 0.4f, BattleSource.None);
    }

    // Moment: 1018003 → 无条件 → 玄气百分比变化
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102005 - ChangeProperty → 自己，玄气，40%，招式
        // ParamList: [1, 20054, 0.4, 3] → 自己，20054(XuanQiPct)，0.4，来源3(招式)
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.4f, BattleSource.None);
    }
}