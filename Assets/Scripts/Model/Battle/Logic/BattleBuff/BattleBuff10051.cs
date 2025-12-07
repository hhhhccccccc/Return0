using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10051 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRecInt)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
