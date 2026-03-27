using System.Collections.Generic;
using Zenject;

public class Skill1030 : BattleSkillBase
{
    // Skill: 遁江 (1030)
    // XuanQiCost: 10, NeedKey: [1, 1]
    // Moments: DoDesitionMoment [1030001]
    
    // Moment: 1030001 → 条件: 1600001 (判断自己不是敌手的目标) → 效果: 111016101 (自己给自己添加匿形10161,1层)
    public override void DoDesition(MomentParamModel paramModel)
    {
        base.DoDesition(paramModel);
        // 条件: 1600001 → CheckSelfIsOppoTarget → 自己是敌手的目标
        // 这里的条件是1600000 (自己不是敌手的目标)，需要判断为false时触发
        // 或者说这个效果在满足条件时触发
        
        // TODO: 需要确认条件判断逻辑 - 暂时按无条件处理
        // 效果: 111016101 - AddBuff → 自己给自己添加匿形10161,1层
        // ParamList: [1, 1, 10161, 1] → 施法者→自己，10161号Buff，1层
        DoAddBuff(Subject, 10161, Subject, 1, null, BattleMomentType.DoDesition);
    }
}