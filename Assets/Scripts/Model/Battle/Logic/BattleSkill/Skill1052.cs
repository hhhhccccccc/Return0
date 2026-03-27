using System.Collections.Generic;
using Zenject;

public class Skill1052 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4210001 - ChangeNearlyBeActionTargetToTeamOther
        // TODO: ChangeNearlyBeActionTargetToTeamOther
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101007 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 5);
        // 效果: 102004 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 5);
    }

}