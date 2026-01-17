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
                    var value = model.GetSelfAttackTruthDamageValue(Subject.EntityID);
                    var healValue = value * pct;
                    target.HealHp(healValue, (BattleSource)Config.ParamList[3].ToInt());
                }
                else
                {
                    var pct = Config.ParamList[2];
                    var value = model.GetSelfAttackTruthDamageValue(Subject.EntityID);
                    var damageValue = value * pct;
                    target.ReduceHp(damageValue, DamageType.InDirect, Subject.EntityID, source: (BattleSource)Config.ParamList[3].ToInt());
                }
            }
        }
    }
}
