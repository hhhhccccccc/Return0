using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_GetDelayShieldBuffByPowerPct : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        if (Subject != null)
        {
            var buffID = Config.ParamList[0].ToInt();
            var power = Subject.GetProperty(BattlePropertyType.Power);
            var pct = Config.ParamList[1];
            BattleBuffManager.AddBuff(Subject, buffID, Subject, 1,
                new List<float> { power * pct });
        }
    }
}