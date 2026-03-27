using System.Collections.Generic;
using Zenject;

public class Skill2042 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3600001 - DamageToTargetByProperty
        // TODO: DamageToTargetByProperty
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101011 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 25);
    }

}