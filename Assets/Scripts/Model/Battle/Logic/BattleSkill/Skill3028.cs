using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3028 : BattleSkillBase
{
    //招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.5f, 50);
    }

    //todo 对自身造成50%造成的伤害
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        
    }

    //玄炁+当前50%（至少15）
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.XuanQi, 0.5f, 15, BattleSource.Skill);
    }
}