using System.Collections.Generic;
using Zenject;

public class Skill3079 : BattleSkillBase
{
    protected override int DontBeCounter()
    {
        return 1;
    }

    
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2500001 - SetAccumulateDamage
        // TODO: SetAccumulateDamage
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111009103 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 13037100 - RemoveBuff
        // TODO: RemoveBuff
    }

}