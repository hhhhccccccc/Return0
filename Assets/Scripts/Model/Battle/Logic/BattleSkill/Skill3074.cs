using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3074 : BattleSkillBase
{
    //招式的刚炁消耗转为当前30%，至多30
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.3f, 30);
    }

    //玄炁+当前30%（至少9），若处于化身类状态则改为玄炁+当前50%（至少15）
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        if (BattleBuffManager.CheckTargetHasAvatarBuff(Subject.EntityID))
        {
            DoHealQiPctByCurr(Subject, BattlePropertyType.XuanQi, 0.5f, 15, BattleSource.Skill);
        }
        else
        {
            DoHealQiPctByCurr(Subject, BattlePropertyType.XuanQi, 0.3f, 9, BattleSource.Skill);
        }
    }
}