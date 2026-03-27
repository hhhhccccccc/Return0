using System.Collections.Generic;
using Zenject;

public class Skill3010 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 10);
        // 效果: 111003101 - AddBuff
        DoAddBuff(Subject, 10031, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}