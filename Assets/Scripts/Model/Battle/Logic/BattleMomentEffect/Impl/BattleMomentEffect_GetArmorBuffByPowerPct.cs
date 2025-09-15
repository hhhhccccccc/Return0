using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_GetArmorBuffByPowerPct : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        if (Subject != null)
        {
            var power = Subject.GetProperty(BattlePropertyType.Power);
            var pct = Config.ParamList[0];
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, 1,
                new List<float> { power * pct });
        }
    }
}