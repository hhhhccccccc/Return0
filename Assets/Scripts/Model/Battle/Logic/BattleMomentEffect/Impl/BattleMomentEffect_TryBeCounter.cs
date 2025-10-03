using cfg;

public class BattleMomentEffect_TryBeCounter : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                if (target.IsAlive() && ParamModel is DamageParamModel { HitHpValue: > 0 } model)
                {
                    target.TryBeCounter(model.AttackID); 
                    Debug($"[扳机效果] 设置破招 {target.EntityID}被破招了");
                }
            }
        }
    }
}