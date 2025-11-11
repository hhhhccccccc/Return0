using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20111 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.PowerPct)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
