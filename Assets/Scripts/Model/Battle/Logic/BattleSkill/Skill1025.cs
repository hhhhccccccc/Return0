using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1025 : BattleSkillBase
{
    // Skill: 1025
    // XuanQiCost: 40, NeedKey: []
    // Moments: ActionWheelStartMoment [1025001], ReleaseSkillActionMoment [1025002], AfterActionMoment [1025003]
    
    // Moment: 1025001 → 无条件 → 给自己添加迅速10041x10，给目标添加缓速20011x10
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 获得10层缓速和10层迅速
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
    }

    // Moment: 1025002 → 无条件 → 给自己添加技增10081x5, 术增10101x5，给目标添加技衰20121x3, 巧衰20131x3
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111008105 - AddBuff → 自己给自己添加技增10081,5层
        DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffShuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1025003 → 无条件 → 我获得5个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}