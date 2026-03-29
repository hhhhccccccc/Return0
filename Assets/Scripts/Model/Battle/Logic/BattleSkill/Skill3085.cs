using System.Collections.Generic;
using Zenject;

public class Skill3085 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 5000202 - RemoveRandomKey
        // TODO: RemoveRandomKey
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122028102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20281, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}