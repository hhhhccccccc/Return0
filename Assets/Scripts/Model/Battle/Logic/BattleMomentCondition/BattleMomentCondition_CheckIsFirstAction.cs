using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentCondition_CheckIsFirstAction : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var state = Config.ParamList[1].ToInt() == 1;
            if (state)
            {
                return target.RoundAlreadyActionTimes == 0;
            }

            return target.RoundAlreadyActionTimes > 0;
        }
        
        return false;
    }
}