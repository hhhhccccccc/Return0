using System.Collections.Generic;
using Zenject;

public class Skill1002 : BattleSkillBase
{
    // Moment: 1002001 → 无条件 → 添加3个随机键
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 400003 - AddRandomKey → 添加3个随机键
        DoAddRandomKey(Subject, 3, ChangeKeyReason.SkillEffect);
    }

    // Moment: 1002002 → 无条件 → 补足到2个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var currentKeyCount = Subject.GetAllKeyCount();
        var needAdd = 2 - currentKeyCount;
        if (needAdd > 0)
        {
            DoAddRandomKey(Subject, needAdd, ChangeKeyReason.SkillEffect);
        }
    }

    // Moment: 1002003 → 无条件 → 添加1个随机键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400001 - AddRandomKey → 添加1个随机键
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}