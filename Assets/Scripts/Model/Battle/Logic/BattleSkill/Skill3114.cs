using System.Collections.Generic;
using Zenject;

public class Skill3114 : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 111007102 - AddBuff
        DoAddBuff(Subject, 10071, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5000201 - RemoveRandomKey
        // TODO: RemoveRandomKey
    }

}