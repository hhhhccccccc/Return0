using cfg;

public class BattleMomentEffect_TryBeCounter : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null && target.IsAlive() && ParamModel is DamageParamModel model && model.HitHpValue > 0)
        {
            target.TryBeCounter(model.AttackID); 
            Debug($"[扳机效果] 设置破招 {target.EntityID}被破招了");
        }
    }
}