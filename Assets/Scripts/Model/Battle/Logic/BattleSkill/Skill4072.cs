using System.Collections.Generic;
using Zenject;

public class Skill4072 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 102010 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 30);
        // 效果: 111018102 - AddBuff
        DoAddBuff(Subject, 10181, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 300001 - AverageGangQiAndXuanQi
        // TODO: AverageGangQiAndXuanQi
    }

}