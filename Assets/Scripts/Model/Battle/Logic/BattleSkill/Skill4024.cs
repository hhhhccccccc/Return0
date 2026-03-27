using System.Collections.Generic;
using Zenject;

public class Skill4024 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 400002 - AddRandomKey
        Subject.AddRandomKey(2, (ChangeKeyReason)4);
        // 效果: 400032 - AddRandomKey
        // TODO: AddRandomKey target=4
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122002105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122008105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20081, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

}