using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20121 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.TechPct)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
