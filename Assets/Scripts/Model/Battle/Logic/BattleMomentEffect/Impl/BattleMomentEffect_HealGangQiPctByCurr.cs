using System;
using cfg;

public class BattleMomentEffect_HealGangQiPctByCurr : BattleMomentEffect
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
                var heal = curr * pct;
                if (Config.ParamList[2] != 0)
                {
                    heal = Math.Max(heal, Config.ParamList[2]);
                }

                target.ChangeProperty(BattlePropertyType.GangQi, heal);
            }
        }
    }
}