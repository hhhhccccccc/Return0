using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2036 : BattleSkillBase
{
    //行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 1);
    }

    //施加1层缓速状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffHuanSu, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //刚炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 10, BattleSource.Skill);
    }
}