using cfg;

public class BattleBuff90003 : BattleBuffBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (Subject.IsAlive())
        {
            if (paramModel is DamageParamModel model)
            {
                if (model.GetOtherDamageType(Subject.EntityID) == DamageType.Direct)
                {
                    var pct = Config.ParamEx[0];
                    var value = model.GetOtherHpValue(Subject.EntityID);
                    var healValue = value * pct;
                    Subject.HealHp(healValue, BattleSource.Skill);
                }
            }
        }
    }
}
