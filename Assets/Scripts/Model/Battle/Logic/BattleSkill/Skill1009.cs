using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1009 : BattleSkillBase
{
    // Moment: 1009001 → 无条件 → 恢复刚气 + 恢复玄气
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        
        // 效果: 101001 - ChangeProperty (刚气)
        // ParamList: [1, 20031, 15, 3] → 自己，刚气，15招式
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 15);
        
        // 效果: 102003 - ChangeProperty (玄气)
        // ParamList: [1, 20051, 15, 3] → 自己，玄气，15招式
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 15);
    }
}