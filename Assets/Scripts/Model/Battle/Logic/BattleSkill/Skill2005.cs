using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2005 : BattleSkillBase
{
    //招式的玄炁消耗转为当前40%，至多40
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.4f, 40);
    }

    //刚炁+当前40%（至少12）
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.GangQi, 0.4f, 12, BattleSource.Skill);
    }
}