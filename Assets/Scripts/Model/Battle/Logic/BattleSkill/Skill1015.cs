using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1015 : BattleSkillBase
{
    // Moment: 1015001 → 无条件 → 添加Buff
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 111001103 - AddBuff → 自己给自己添加心眼10011,3层
        DoAddBuff(Subject, 10011, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    // Moment: 1015002 → 无条件 → 恢复刚气60
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101002 - ChangeProperty → 恢复刚气60
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 60);
    }

    // Moment: 1015003 → 无条件 → 添加多个Buff
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 111009101 - AddBuff → 自己给自己添加武增10091,1层
        DoAddBuff(Subject, 10091, Subject, 1, null, BattleMomentType.AfterAction);
        
        // 效果: 111007102 - AddBuff → 自己给自己添加技增10081,1层
        DoAddBuff(Subject, 10081, Subject, 1, null, BattleMomentType.AfterAction);
    }
}