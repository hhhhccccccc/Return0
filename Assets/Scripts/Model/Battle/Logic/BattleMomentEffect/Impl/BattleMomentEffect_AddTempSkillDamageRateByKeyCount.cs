using System;
using cfg;

public class BattleMomentEffect_AddTempSkillDamageRateByKeyCount : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var keyCount = target.GetKeyCount();
            keyCount = Math.Max(keyCount - Config.ParamList[1].ToInt(), 0);
            var addValue = keyCount * Config.ParamList[2];
            target.AddTempSkillDamageValue(addValue);
        }
    }
}