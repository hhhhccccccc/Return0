using cfg;
using Zenject;

public class BattleMomentEffect_DamageToTarget : BattleMomentEffect
{
    [Inject] private IPoolManager PoolManager;
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        var target = GetUnitByParamID(Config.ParamList[1]);
        if (subject != null && target != null)
        {
            var damageRate = Config.ParamList[3];
            var damageType = (DamageType)Config.ParamList[4].ToInt();
            var damageSource = (BattleSource)Config.ParamList[5].ToInt();
            var damageValue = subject.GetSkillDamageValue(target, damageType, damageSource, damageRate);
            var damageParamModel = PoolManager.GetClass<DamageParamModel>();
            damageParamModel.AttackDamageValue = damageValue;
            damageParamModel.AttackDamageType = damageType;
            damageParamModel.AttackSource = damageSource;
            target.BeDamage(ref damageParamModel);
            PoolManager.RecycleClass(damageParamModel);
        }
    }
}