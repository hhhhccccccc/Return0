using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10111 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.CleverPct)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
