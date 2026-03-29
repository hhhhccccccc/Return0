using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1008 : BattleSkillBase
{
    // Moment: 1008001 → 无条件 → 自己加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900001 - ChangeActionWheel
        // ParamList: [1, 1] → 自己，加快1息
        DoChangeActionWheel(Subject, 1);
    }

    // Moment: 1008002 → 无条件 → 给自己添加反击10011,1层
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111003101 - AddBuff
        // ParamList: [1, 1, 10031, 1] → 自己给自己添加反击10011,1层
        DoAddBuff(Subject, 10031, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1008003 → 无条件 → 3息内反击buff不会低于1层
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 119000904 - AddBuff
        // ParamList: [1, 1, 90009, 4] → 3息内反击buff不会低于1层
        DoAddBuff(Subject, 90009, Subject, 4, null, BattleMomentType.AfterAction);
    }
}