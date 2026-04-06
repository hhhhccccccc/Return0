using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10201 : BattleBuffBase
{
    /// <summary>
    /// 全部的破、防的提升效果增加30%
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.BreakAddPct)
        {
            return GetConfigParamFloat(0);
        }

        if (propertyType == BattlePropertyType.DefendAddPct)
        {
            return GetConfigParamFloat(1);
        }

        return 0;
    }
}
