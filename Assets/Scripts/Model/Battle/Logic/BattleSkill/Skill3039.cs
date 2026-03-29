using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3039 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        if (CheckBuffTypeCount(Subject, BuffType.Abnormal, 0, DataRelation.DaYu))
        {
            return 1;
        }
        
        return 0;
    }
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5901222 - AddHasBuff
        // TODO: AddHasBuff
    }

}