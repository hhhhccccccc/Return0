using cfg;

public class BattleMomentEffect_ChangeHpByAttackDamage : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0 && ParamModel is DamageParamModel model)
        {
            foreach (var target in targetList)
            {
                var op = Config.ParamList[1].ToInt();
                if (op == 1)
                {
                    var pct = Config.ParamList[2];
                    var value = model.AttackDamageValue;
                    var healValue = value * pct;
                    target.ChangeProperty(BattlePropertyType.Hp, healValue);
                }
                else
                {
                    var pct = Config.ParamList[2];
                    var value = model.AttackDamageValue;
                    var damageValue = value * pct;
                    target.ReduceHp(damageValue, DamageType.InDirect);
                }
            }
        }
    }
}