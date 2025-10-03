using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_GetDelayShieldBuffByPowerPct : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var buffID = Config.ParamList[1].ToInt();
            var power = target.GetProperty(BattlePropertyType.Power);
            var pct = Config.ParamList[2];
            BattleBuffManager.AddBuff(target, buffID, target, 1,
                new List<float> { power * pct });
        }
    }
}