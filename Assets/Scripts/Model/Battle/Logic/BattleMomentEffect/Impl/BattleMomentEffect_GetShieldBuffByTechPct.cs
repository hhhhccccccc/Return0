using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_GetShieldBuffByTechPct : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var power = target.GetProperty(BattlePropertyType.Tech);
            var pct = Config.ParamList[1];
            BattleBuffManager.AddBuff(target, GameConst.Battle.ShieldBuffID, target, 1,
                new List<float> { power * pct });
        }
    }
}