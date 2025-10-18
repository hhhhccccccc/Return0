using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_IgnoreDirectKillingDamage : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var state = Config.ParamList[1].ToInt() == 1;
            foreach (var target in targetList)
            {
                target.AddIgnoreDirectKillingDamage(state ? 1 : -1);
            }
        }
    }
}