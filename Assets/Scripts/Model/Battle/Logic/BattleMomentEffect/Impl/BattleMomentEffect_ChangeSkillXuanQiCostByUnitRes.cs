using System;
using cfg;

public class BattleMomentEffect_ChangeSkillXuanQiCostByUnitRes : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var curr = target.GetSkillBase.GetXuanQiCost();
            var pct = Config.ParamList[1];
            var cost = curr * pct;
            if (Config.ParamList[2] != 0)
            {
                cost = Math.Min(cost, Config.ParamList[2]);
            }
            target.GetSkillBase.SetXuanQiCost(cost);
        }
    }
}