using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2046 : BattleSkillBase
{
    //招式的玄炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.5f, 50);
    }

    //若互为目标消耗双方10刚炁
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        if (CheckMutualGoal(Subject, clashUnit))
        {
            DoChangeProperty(Subject, BattlePropertyType.GangQi, -0.1f, BattleSource.Skill);
            DoChangeProperty(clashUnit, BattlePropertyType.GangQi, -0.1f, BattleSource.Skill);
        }
    }
}