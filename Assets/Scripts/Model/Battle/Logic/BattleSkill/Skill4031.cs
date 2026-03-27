using System.Collections.Generic;
using Zenject;

public class Skill4031 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 400007 - AddRandomKey
        Subject.AddRandomKey(7, (ChangeKeyReason)4);
    }

}