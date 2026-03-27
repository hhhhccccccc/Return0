using System.Collections.Generic;
using Zenject;

public class Skill1041 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 104003 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.Physique, 40);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102008 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 40);
    }

}