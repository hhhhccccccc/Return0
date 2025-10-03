using cfg;
using Zenject;

public class BattleMomentEffect_DamageToTarget : BattleMomentEffect
{
    [Inject] private IPoolManager PoolManager;
    protected override void OnEffect()
    {
        var subjectList = GetUnitByParamID(Config.ParamList[0]);
        var targetList = GetUnitByParamID(Config.ParamList[1]);
        if (subjectList.Count > 0 && targetList.Count > 0)
        {
            var damageRate = Config.ParamList[3];
            var damageType = (DamageType)Config.ParamList[4].ToInt();
            var damageSource = (BattleSource)Config.ParamList[5].ToInt();
            var damageValue = subjectList[0].GetSkillDamageValue(targetList[0], damageType, damageSource, damageRate);
            var damageParamModel = PoolManager.GetClass<DamageParamModel>();
            damageParamModel.AttackDamageValue = damageValue;
            damageParamModel.AttackDamageType = damageType;
            damageParamModel.AttackSource = damageSource;
            targetList[0].BeDamage(ref damageParamModel);
            PoolManager.RecycleClass(damageParamModel);
        }
    }
}