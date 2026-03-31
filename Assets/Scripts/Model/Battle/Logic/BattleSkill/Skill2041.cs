using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2041 : BattleSkillBase
{
    //招式的玄炁消耗转为当前60%，至多60
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.6f, 60);
    }

    //施加3层寒沁状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffHanXin, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    //刚炁+当前30%（至少9）
    public override void AfterAction(MomentParamModel paramModel)
    {
         DoHealQiPctByCurr(Subject, BattlePropertyType.GangQi, 0.3f, 9, BattleSource.Skill);
    }
}