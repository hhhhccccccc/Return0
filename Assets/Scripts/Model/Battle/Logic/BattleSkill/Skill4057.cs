using System.Collections.Generic;
using Zenject;

public class Skill4057 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 151001101 - AddBuff
        // TODO: AddBuff [caster=1, target=5]
        // 效果: 151011102 - AddBuff
        // TODO: AddBuff [caster=1, target=5]
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122018102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20181, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122019102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20191, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}