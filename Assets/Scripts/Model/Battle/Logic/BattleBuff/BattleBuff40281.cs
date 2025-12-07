using System;
using cfg;
using Zenject;

public class BattleBuff40281 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.TechInt)
        {
            return Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        }

        return 0;
    }
}
