using System.Collections.Generic;
using Zenject;

public class Skill1042 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 104004 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.Physique, 55);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102009 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 55);
    }

}