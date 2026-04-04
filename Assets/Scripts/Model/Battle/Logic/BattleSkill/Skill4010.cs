using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4010 : BattleSkillBase
{
    //对目标施加5层法式禁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffFaShiJin, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }
}