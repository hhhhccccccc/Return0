using System;
using cfg;
using Zenject;

public class BattleMomentEffect_RecoverRoundBeDamagePct : BattleMomentEffect
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                target.HealHp(Config.ParamList[1] * target.RoundBeDamageValue, BattleSource.Skill);
            }
        }
    }
}
