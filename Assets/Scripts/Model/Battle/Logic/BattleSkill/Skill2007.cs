using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2007 : BattleSkillBase
{
    //,施加3层术式禁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffShuShiJin, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
    //刚炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 10, BattleSource.Skill);
    }
}