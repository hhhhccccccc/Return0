using System.Collections.Generic;
using Zenject;

public class Skill4060 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122010105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 107002 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.Hp, -20);
    }

}