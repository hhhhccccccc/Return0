using System.Collections.Generic;
using Zenject;

public class Skill4082 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4402100 - ClearBuffByType
        // TODO: ClearBuffByType target=2
        // 效果: 122035104 - AddBuff
        if (Target != null) DoAddBuff(Target, 20351, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

}