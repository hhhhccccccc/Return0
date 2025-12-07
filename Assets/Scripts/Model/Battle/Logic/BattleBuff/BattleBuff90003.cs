using cfg;

public class BattleBuff90003 : BattleBuffBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (Subject.IsAlive())
        {
            if (paramModel is DamageParamModel { AttackDamageType: DamageType.Direct } model)
            {
                var pct = Config.ParamEx[0];
                var value = model.HitHpValue;
                var healValue = value * pct;
                Subject.HealHp(healValue, BattleSource.Skill);
            }
        }
    }
}
