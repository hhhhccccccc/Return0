using cfg;

public class BattleMomentEffect_HealHpByDirectDamage : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var healTarget = GetUnitByParamID(unitParamID);
        if (healTarget != null && ParamModel is DamageParamModel model)
        {
            var healPct = Config.ParamList[1];
            var healValue = model.AttackDamageValue * healPct;
            healTarget.ChangeProperty(BattlePropertyType.Hp, healValue);
        }
    }
}