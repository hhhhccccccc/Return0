using System.Collections.Generic;
using Zenject;

public class Skill3029 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5000202 - RemoveRandomKey
        // TODO: RemoveRandomKey
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102003 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 15);
    }

}