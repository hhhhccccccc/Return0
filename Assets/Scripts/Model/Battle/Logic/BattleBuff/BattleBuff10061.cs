using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10061 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.XuanQiRecInt)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
