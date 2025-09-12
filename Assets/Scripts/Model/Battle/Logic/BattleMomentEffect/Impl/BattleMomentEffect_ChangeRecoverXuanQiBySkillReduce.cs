using System;
using cfg;

public class BattleMomentEffect_ChangeRecoverXuanQiBySkillReduce : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            target.ChangeRecoverXuanQiBySkillReduce(Config.ParamList[1]);
        }
    }
}