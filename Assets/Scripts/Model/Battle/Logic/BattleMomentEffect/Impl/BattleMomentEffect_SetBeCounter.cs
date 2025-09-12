using cfg;

public class BattleMomentEffect_SetBeCounter : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            target.SetBeCounter(true); 
            Debug($"[扳机效果] 设置破招 {target.EntityID}被破招了");
        }
    }
}