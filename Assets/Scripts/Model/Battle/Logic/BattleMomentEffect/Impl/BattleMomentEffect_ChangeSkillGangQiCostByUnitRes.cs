using System;
using cfg;

public class BattleMomentEffect_ChangeSkillGangQiCostByUnitRes : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var skillBase = target.GetSkill();
            if (skillBase != null)
            {
                var curr = target.GetProperty(BattlePropertyType.GangQi);
                var pct = Config.ParamList[1];
                var cost = curr * pct;
                if (Config.ParamList[2] != 0)
                {
                    cost = Math.Min(cost, Config.ParamList[2]);
                }
                skillBase.SetGangQiCost(cost);
            }
        }
    }
}