using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20301 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.GangQiRedInt)
        {
            return LayerCount * Config.ParamEx[0].ToInt();
        }

        return 0;
    }
}
