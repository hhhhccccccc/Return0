using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20091 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRecInt)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
