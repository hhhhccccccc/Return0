using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10081 : BattleBuffBase
{
    /// <summary>
    /// 每层使技增加10%
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TechPct)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
