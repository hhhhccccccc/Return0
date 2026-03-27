using System.Collections.Generic;
using Zenject;

public class Skill3012 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122002110 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 10, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102002 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 20);
    }

}