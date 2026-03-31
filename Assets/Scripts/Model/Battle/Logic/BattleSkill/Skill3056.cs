using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3056 : BattleSkillBase
{
    protected override int DontBeCounter()
    {
        if (CheckRoundBeDirectDamageTimes(Subject, 2, DataRelation.XiaoYu))
        {
            return 1;
        }
        
        return 0;
    }
    
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122032101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20321, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}