using System.Collections.Generic;
using Zenject;

public class Skill2040 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101005 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 20);
        // 效果: 119001001 - AddBuff
        DoAddBuff(Subject, 90010, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}