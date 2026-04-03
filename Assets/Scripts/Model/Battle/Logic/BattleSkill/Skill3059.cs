using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3059 : BattleSkillBase
{
    //招式的玄炁消耗转为当前80%，至多80
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.8f, 80);
    }

    //获得1层武增状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}