using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1030 : BattleSkillBase
{
    // Skill: 遁江 (1030)
    // XuanQiCost: 10, NeedKey: [1, 1]
    // Moments: DoDesitionMoment [1030001]
    
    // Moment: 1030001 → 条件: 1600001 (判断自己不是敌手的目标) → 效果: 111016101 (自己给自己添加匿形10161,1层)
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (CheckSelfIsOppoTarget(false))
        {
            DoAddBuff(Subject, GameConst.Battle.BuffNiXing, Subject, 1, null, BattleMomentType.DoDesitionAction);
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 20);
    }
}