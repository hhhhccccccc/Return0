using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30301 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.Power || propertyType == BattlePropertyType.Tech)
        {
            return Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        }

        return 0;
    }
}
