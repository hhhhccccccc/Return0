using System.Collections.Generic;
using Zenject;

public class Skill1024 : BattleSkillBase
{
    // Skill: 焚羽凌霄 (1024)
    // XuanQiCost: 80, NeedKey: [1, 4, 2, 3]
    // Moments: ActionWheelStartMoment [1024001], ReleaseSkillActionMoment [1024002], AfterActionMoment [1024003]
    
    // Moment: 1024001 → 无条件 → 给自己添加多个增益Buff + 给目标添加多个减益Buff
    public override void ActionWheelStart(MomentParamModel paramModel)
    {
        base.ActionWheelStart(paramModel);
        // 给自己添加: 迅速10041x10, 刚聚10051x5, 玄聚10061x5
        DoAddBuff(Subject, 10041, Subject, 10, null, BattleMomentType.ActionWheelStart);
        DoAddBuff(Subject, 10051, Subject, 5, null, BattleMomentType.ActionWheelStart);
        DoAddBuff(Subject, 10061, Subject, 5, null, BattleMomentType.ActionWheelStart);
        
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // 给目标添加: 缓速20011x10, 力衰20111, 技衰20121, 刚聚10051, 玄聚10061
                DoAddBuff(otherUnit, 20011, Subject, 10, null, BattleMomentType.ActionWheelStart);
                DoAddBuff(otherUnit, 20111, Subject, 5, null, BattleMomentType.ActionWheelStart);
                DoAddBuff(otherUnit, 20121, Subject, 5, null, BattleMomentType.ActionWheelStart);
                DoAddBuff(otherUnit, 10051, Subject, 5, null, BattleMomentType.ActionWheelStart);
                DoAddBuff(otherUnit, 10061, Subject, 5, null, BattleMomentType.ActionWheelStart);
            }
        }
    }

    // Moment: 1024002 → 无条件 → 给自己和目标添加多个Buff
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 给自己添加: 力增10071x5, 技增10081x5, 武增10091x5, 术增10101x5, 巧增10111x5
        DoAddBuff(Subject, 10071, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, 10081, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, 10091, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, 10101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, 10111, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // 给目标添加: 武增10091, 术增10101, 巧增10111, 刚聚10051, 玄聚10061, 技衰20121
                DoAddBuff(otherUnit, 10091, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
                DoAddBuff(otherUnit, 10101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
                DoAddBuff(otherUnit, 10111, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
                DoAddBuff(otherUnit, 10051, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
                DoAddBuff(otherUnit, 10061, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
                DoAddBuff(otherUnit, 20121, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }

    // Moment: 1024003 → 无条件 → 设置刚气玄气各50 + 获得5个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 105001 - SetProperty → 设置自己刚气为50
        // 效果: 106001 - SetProperty → 设置自己玄气为50
        // 效果: 400005 - AddRandomKey → 我获得5个键
        DoSetProperty(Subject, 20031, 50, ChangePropertyReason.Skill);
        DoSetProperty(Subject, 20051, 50, ChangePropertyReason.Skill);
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}