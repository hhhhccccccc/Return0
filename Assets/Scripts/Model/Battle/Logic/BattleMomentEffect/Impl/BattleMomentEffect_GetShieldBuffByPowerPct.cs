using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_GetShieldBuffByPowerPct : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var power = target.GetProperty(BattlePropertyType.Power);
                var pct = Config.ParamList[1];
                BattleBuffManager.AddBuff(target, GameConst.Battle.ShieldBuffID, target, (power * pct).ToInt());
            }
        }
    }
}