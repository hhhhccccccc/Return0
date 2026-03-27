using System.Collections.Generic;
using Zenject;

public class Skill1016 : BattleSkillBase
{
    // Moment: 1016002 → 无条件 → 添加多个Buff
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111009102 - AddBuff → 自己给自己添加武增10091,2层
        DoAddBuff(Subject, 10091, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 111007103 - AddBuff → 自己给自己添加力增10071,3层
        DoAddBuff(Subject, 10071, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 111005104 - AddBuff → 自己给自己添加刚聚10051,4层
        DoAddBuff(Subject, 10051, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1016003 → 无条件 → 添加5个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400005 - AddRandomKey → 我获得5个键
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}