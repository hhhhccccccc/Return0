using System;
using System.Collections.Generic;
using cfg;

public class BattleMomentEffect_ExitBattle : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            
        }
    }
}