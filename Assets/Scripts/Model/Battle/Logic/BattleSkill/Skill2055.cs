using System.Collections.Generic;
using Zenject;

public class Skill2055 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 200004 - ChangeHpByAttackDamage
        // TODO: ChangeHpByAttackDamage
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101013 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 70);
    }

}