using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20121 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TechPct)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
