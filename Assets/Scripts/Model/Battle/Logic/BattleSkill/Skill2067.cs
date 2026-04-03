using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill2067 : BattleSkillBase
{
    public override bool IsTrueDamage(DamageParamModel model) => true;

    //获得5层巧增
    public override void SelfActionWheelStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffQiaoZeng, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
    }

    //招式的玄炁消耗转为当前100%，至多100
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 1, 100);
    }
    
    //刚炁+当前100%（至少30）

    public override void AfterAction(MomentParamModel paramModel)
    {
        DoHealQiPctByCurr(Subject, BattlePropertyType.GangQi, 1, 30, BattleSource.Skill);
    }
}