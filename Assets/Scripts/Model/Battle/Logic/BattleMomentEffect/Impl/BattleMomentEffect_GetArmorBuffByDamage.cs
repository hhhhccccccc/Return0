using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ConvertDamageToArmorBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        if (Subject != null && ParamModel is DamageParamModel model)
        {
            var value = model.HitHpValue;
            var isDie = Subject.ReduceHp(value, DamageType.InDirect);
            if (!isDie)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, 1,
                    new List<float> { value });
            }
        }
    }
}