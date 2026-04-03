using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3057 : BattleSkillBase
{
    //获得3层避殃状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    //解除目标2个增益状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuffByType(Target, BuffType.Gain, 2);
    }
}