using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1026 : BattleSkillBase
{
    // Skill: 鸩姿 (1026)
    // GangQiCost: 20, NeedKey: [2, 3, 3]
    // Moments: ActionWheelStartMoment [1026001], ReleaseSkillActionMoment [1026002]
    
    // Moment: 1026001 → 无条件 → 自己补满键
    public override void SelfActionWheelStart()
    {
        // 效果: 900000 - AddRandonKeyToDefineCount → 自己补满键
        // ParamList: [1, 0] → 施法者，补满(0)
        DoAddRandomKeyToDefineCount(Subject, 0);
    }

    // Moment: 1026002 → 无条件 → 自己获得1次行动机会 + Buff下回合开始移除全部的键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3400001 - AddActionTimes → 自己获得1次行动机会
        // ParamList: [1, 1] → 施法者，1次
        DoAddActionTimes(Subject, 1);
        
        // 效果: 119000401 - AddBuff → Buff下回合开始移除全部的键
        // ParamList: [1, 1, 90004, 1] → 施法者→自己，90004号Buff，1层
        DoAddBuff(Subject, 90004, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}