using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20101 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRecInt)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
