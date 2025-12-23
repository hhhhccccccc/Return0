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
            var value = model.GetOtherHpValue(Subject.EntityID);
            var isDie = Subject.ReduceHp(value, DamageType.InDirect, Subject.EntityID, source: BattleSource.Skill);
            if (!isDie)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, value.ToInt(), null, MomentType);
            }
        }
    }
}