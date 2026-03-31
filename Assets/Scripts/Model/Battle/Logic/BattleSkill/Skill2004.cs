using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2004 : BattleSkillBase
{
    //招式的玄炁消耗转为当前30%，至多30
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.3f, 30);
    }

    //刚炁+当前30%（至少9）
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.GangQi, 0.3f, 9, BattleSource.Skill);
    }
}