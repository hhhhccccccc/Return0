using System.Collections.Generic;
using Zenject;

public class Skill2068 : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 112031105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20311, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122007103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20071, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122002103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}