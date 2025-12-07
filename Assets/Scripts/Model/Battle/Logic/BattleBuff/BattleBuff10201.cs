using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10201 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.BreakAddPct)
        {
            return Config.ParamEx[0];
        }

        if (propertyType == BattlePropertyType.DefendAddPct)
        {
            return Config.ParamEx[1];
        }

        return 0;
    }
}
