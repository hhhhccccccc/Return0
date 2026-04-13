using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4006 : BattleSkillBase
{
    //玄炁+30
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 30, BattleSource.Skill);
    }

    //获得4层借法
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffJieFa, Subject, 4, null, BattleMomentType.AfterAction);
    }
}