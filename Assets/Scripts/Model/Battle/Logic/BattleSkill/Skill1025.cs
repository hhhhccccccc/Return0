using System.Collections.Generic;
using Zenject;

public class Skill1025 : BattleSkillBase
{
    // Skill: 1025
    // XuanQiCost: 40, NeedKey: []
    // Moments: ActionWheelStartMoment [1025001], ReleaseSkillActionMoment [1025002], AfterActionMoment [1025003]
    
    // Moment: 1025001 → 无条件 → 给自己添加迅速10041x10，给目标添加缓速20011x10
    public override void ActionWheelStart(MomentParamModel paramModel)
    {
        base.ActionWheelStart(paramModel);
        // 效果: 111004110 - AddBuff → 自己给自己添加迅速10041,10层
        DoAddBuff(Subject, 10041, Subject, 10, null, BattleMomentType.ActionWheelStart);
        
        // 效果: 112001110 - AddBuff → 自己给目标添加缓速20011,10层
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20011, Subject, 10, null, BattleMomentType.ActionWheelStart);
            }
        }
    }

    // Moment: 1025002 → 无条件 → 给自己添加技增10081x5, 术增10101x5，给目标添加技衰20121x3, 巧衰20131x3
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111008105 - AddBuff → 自己给自己添加技增10081,5层
        DoAddBuff(Subject, 10081, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 111010105 - AddBuff → 自己给自己添加术增10101,5层
        DoAddBuff(Subject, 10101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // 效果: 142012105 - AddBuff → 自己给目标添加技衰20121,3层
                DoAddBuff(otherUnit, 20121, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
                
                // 效果: 142014105 - AddBuff → 自己给目标添加巧衰20131,3层
                DoAddBuff(otherUnit, 20131, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }

    // Moment: 1025003 → 无条件 → 我获得5个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400005 - AddRandomKey → 我获得5个键
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}