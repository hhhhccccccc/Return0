using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill2066 : BattleSkillBase
{
    //招式的玄炁消耗转为当前70%，至多70
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.7f, 70);
    }

    //施加等同目标行动招式包含的键数量的伤口状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        var clashSkill = clashUnit.GetSkill();
        var keyCount = clashSkill.GetKeyCostList.Count;
        DoAddBuff(clashUnit, GameConst.Battle.BuffShangKou, Subject, keyCount, null, BattleMomentType.BeforeClash);
    }
}