using cfg;

public class BattleMomentEffect_HealHpByDirectDamagePct : BattleMomentEffect
{
    protected override void OnEffect()
    {
        if (ParamModel is DamageParamModel model)
        {
            var healPct = Config.ParamList[1];
            var value = model.HitDamageValue;
            var healValue = value * healPct;
            Subject.ChangeProperty(BattlePropertyType.Hp, healValue);
        }
    }
}