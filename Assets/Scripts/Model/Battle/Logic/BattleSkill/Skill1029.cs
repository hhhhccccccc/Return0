using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1029 : BattleSkillBase
{
    // Skill: 周旋 (1029)
    // XuanQiCost: 25, NeedKey: [3, 3, 3]
    // Moments: ReleaseSkillActionMoment [1029001], AfterActionMoment [1029002]
    
    // Moment: 1029001 → 无条件 → 自己给自己添加技增10081,1层
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111002101 - AddBuff → 自己给自己添加技增10081,1层
        // ParamList: [1, 1, 10081, 1] → 施法者→自己，10081号Buff，1层
        DoAddBuff(Subject, 10081, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1029002 → 无条件 → 我获得4个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400004 - AddRandomKey → 我获得4个键
        // ParamList: [1, 4, 4] → 施法者，4个键，上限4
        DoAddRandomKey(Subject, 4, ChangeKeyReason.SkillEffect);
    }
}