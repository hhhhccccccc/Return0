using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4076 : BattleSkillBase
{
    //本次行动延迟3息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -3);
    }

    //todo 最后一个行动的角色行动后受到150%技的伤害，并获得3层力衰和1层破绽

    //todo 与杀式交锋则敌手受到150%技的伤害，施加3层力衰和1层破绽
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        if (CheckSkillIsKillingStyle(clashUnit, true))
        {
            DoAddBuff(clashUnit, GameConst.Battle.BuffLiShuai, Subject, 3, null, BattleMomentType.BeforeClash);
            DoAddBuff(clashUnit, GameConst.Battle.BuffPoZhan, Subject, 1, null, BattleMomentType.BeforeClash);
        }
    }
}