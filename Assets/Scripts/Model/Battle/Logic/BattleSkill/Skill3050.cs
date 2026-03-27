using System.Collections.Generic;
using Zenject;

public class Skill3050 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102012 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 25);
        // 效果: 119001501 - AddBuff
        DoAddBuff(Subject, 90015, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}