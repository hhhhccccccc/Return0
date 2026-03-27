using System.Collections.Generic;
using Zenject;

public class Skill3041 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122001103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20011, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122009103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 10);
    }

}