using System.Collections.Generic;
using Zenject;

public class Skill1014 : BattleSkillBase
{
    // Moment: 1014002 → 无条件 → 转换两层异常为增益
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3900002 - ConvertBuffAbnormalToGain → 自己转换两层异常为增益
        // ParamList: [1, 200001, 2] → 自己，200001(增益池)，2(转换2层)
        DoConvertBuffAbnormalToGain(Subject, 200001, 2);
    }

    // Moment: 1014003 → 无条件 → 添加6个键 + 添加Buff
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400006 - AddRandomKey → 我获得6个键
        DoAddRandomKey(Subject, 6, ChangeKeyReason.SkillEffect);
        
        // 效果: 111001102 - AddBuff → 自己给自己添加心眼10011,2层
        DoAddBuff(Subject, 10011, Subject, 2, null, BattleMomentType.AfterAction);
    }
}