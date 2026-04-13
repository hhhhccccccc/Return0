using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3006 : BattleSkillBase
{
    //招式的刚炁消耗转为当前70%，至多70
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.7f, 70);
    }

    //玄炁+当前70%（至少21）
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.XuanQi, 0.7f, 21, BattleSource.Skill);
    }
}