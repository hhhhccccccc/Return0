using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3005 : BattleSkillBase
{
    //招式的刚炁消耗转为当前40%，至多40
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.4f, 40);
    }

    //玄炁+当前40%（至少21）
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.XuanQi, 0.4f, 21, BattleSource.Skill);
    }
}