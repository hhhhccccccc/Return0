using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10051 : BattleBuffBase
{
    /// <summary>
    /// 每层使获得的刚炁增加1
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRecInt)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
