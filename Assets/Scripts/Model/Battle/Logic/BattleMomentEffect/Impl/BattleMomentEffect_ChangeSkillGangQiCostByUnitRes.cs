using System;
using cfg;

public class BattleMomentEffect_ChangeSkillGangQiCostByUnitRes : BattleMomentEffect
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
}