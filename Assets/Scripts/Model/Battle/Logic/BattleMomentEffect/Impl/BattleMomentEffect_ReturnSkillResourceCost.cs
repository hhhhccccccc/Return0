using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ReturnSkillResourceCost : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                target.ReturnSkillResourceCost(Config.ParamList[1].ToInt() == 1, Config.ParamList[2].ToInt() == 1, Config.ParamList[3].ToInt() == 1);
            }
        }
    }
}