using System;
using cfg;

public class BattleMomentEffect_ChangeRecoverGangQiBySkillReduce : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            target.ChangeRecoverGangQiBySkillReduce(Config.ParamList[1]);
            Debug($"[扳机效果] 改变招式获得的气偏移值 目标 : {target.EntityID}, 值 : {Config.ParamList[1]}");
        }
    }
}