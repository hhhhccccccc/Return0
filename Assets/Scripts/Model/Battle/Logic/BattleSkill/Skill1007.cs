using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1007 : BattleSkillBase
{
    // Moment: 1007001 → 无条件 → 自己加快2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900002 - ChangeActionWheel
        // ParamList: [1, 2] → 自己，加快2息
        DoChangeActionWheel(Subject, 2);
    }

    // Moment: 1007002 → 无条件 → 添加增益Buff + 移除异常Buff
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        
        // 效果: 121004102 - AddBuff
        // ParamList: [1, 2, 10041, 2] → 自己给目标添加迅速10041,2层
        DoAddBuff(Subject, 10041, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 22001100 - RemoveBuff
        // ParamList: [2, 20011, 0] → 目标，缓速，所有层数
        DoRemoveBuff(Subject, 20011);
    }

    // Moment: 1007003 → 无条件 → 恢复玄气 + 双方获得键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        
        // 效果: 102002 - ChangeProperty (玄气)
        // ParamList: [1, 20051, 20, 3] → 自己，玄气，20招式
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 20);
        
        // 效果: 400002 - AddRandomKey (自己获得2个键)
        // ParamList: [1, 2, 4] → 我获得2个键
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
        
        // 效果: 400012 - AddRandomKey (对方获得2个键)
        // ParamList: [2, 2, 4] → 对方获得2个键
        DoAddRandomKey(Target, 2, ChangeKeyReason.SkillEffect);
    }
}