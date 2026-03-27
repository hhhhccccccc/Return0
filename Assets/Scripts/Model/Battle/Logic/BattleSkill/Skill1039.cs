using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1039 : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 119000601 - AddBuff
        DoAddBuff(Subject, 90006, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4500002 - AddGainBuffByBuffIDCount
        // TODO: AddGainBuffByBuffIDCount buffID=20341 count=2 gain=200002
    }

}