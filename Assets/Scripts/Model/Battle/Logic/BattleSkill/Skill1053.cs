using System.Collections.Generic;
using Zenject;

public class Skill1053 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111009103 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111007103 - AddBuff
        DoAddBuff(Subject, 10071, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 107001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.Hp, 10);
        // 效果: 108001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.Neili, 30);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 3800003 - GetShieldBuffByPowerPct
        DoGetShieldBuff(Subject, 0.5, BattleMomentType.AfterAction);
    }

}