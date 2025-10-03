using System;
using cfg;

public class BattleMomentEffect_HealXuanQiPctByCurr : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var skillBase = target.GetSkill();
                if (skillBase != null)
                {
                    var curr = target.GetProperty(BattlePropertyType.XuanQi);
                    var pct = Config.ParamList[1];
                    var heal = curr * pct;
                    if (Config.ParamList[2] != 0)
                    {
                        heal = Math.Max(heal, Config.ParamList[2]);
                    }

                    target.ChangeProperty(BattlePropertyType.XuanQi, heal);
                }
            }
        }
    }
}