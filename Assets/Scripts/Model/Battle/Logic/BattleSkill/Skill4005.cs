using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4005 : BattleSkillBase
{
    //招式的玄炁消耗转为当前40%，至多40
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.4f, 40);
    }

    //获得1层隐魂
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffYinHun, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}