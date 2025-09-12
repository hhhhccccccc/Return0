using cfg;

public class BattleMomentEffect_SetDontBeCounter : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var state = Config.ParamList[1].ToInt() == 1;
            target.SetDontBeCounter(state ? 1 : -1);
            Debug($"[扳机效果] 设置是否不可被破招 目标 : {target.EntityID}, 效果 : {state}");
        }
    }
}