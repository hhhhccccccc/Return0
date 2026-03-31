using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2006 : BattleSkillBase
{
    //招式的玄炁消耗转为当前70%，至多70
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.7f, 70);
    }

    //刚炁+当前70%（至少21）
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.GangQi, 0.7f, 21, BattleSource.Skill);
    }
}