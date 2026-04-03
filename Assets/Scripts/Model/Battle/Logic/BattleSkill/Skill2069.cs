using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2069 : BattleSkillBase
{
    //清除自身1层异常状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoClearBuffByType(Subject, BuffType.Abnormal, 1);
    }

    //获得1层避殃状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}