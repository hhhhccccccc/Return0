using System.Collections.Generic;
using Zenject;

public class Skill4038 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 5000401 - RemoveRandomKey
        // TODO: RemoveRandomKey
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5000202 - RemoveRandomKey
        // TODO: RemoveRandomKey
        // 效果: 600005 - RandomAllKey
        // TODO: RandomAllKey
        // 效果: 122002103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}