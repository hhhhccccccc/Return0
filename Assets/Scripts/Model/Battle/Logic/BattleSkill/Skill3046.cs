using System.Collections.Generic;
using Zenject;

public class Skill3046 : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 5600001 - RemoveKey
        // TODO: RemoveKey
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 112009104 - AddBuff
        DoAddBuff(Subject, 20091, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

}