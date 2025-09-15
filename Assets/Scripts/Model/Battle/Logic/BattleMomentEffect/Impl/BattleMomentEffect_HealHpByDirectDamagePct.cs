using cfg;

public class BattleMomentEffect_HealHpByDirectDamagePct : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0].ToInt();
        if (ParamModel is DamageParamModel model)
        {
            var healTarget = GetUnitByParamID(unitParamID);
            var healPct = Config.ParamList[1];
            float value = 0;
            if (unitParamID == 1)
            {
                value = model.HitDamageValue;
            }
            else if (unitParamID == 2)
            {
                value = model.AttackDamageValue;
            }
           
            var healValue = value * healPct;
            healTarget.ChangeProperty(BattlePropertyType.Hp, healValue);
        }
    }
}