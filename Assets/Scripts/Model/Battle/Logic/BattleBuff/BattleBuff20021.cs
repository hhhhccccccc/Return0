using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20021 : BattleBuffBase
{
    protected override int OnGetChangeActionWheel()
    {
        return LayerCount * GetConfigParamInt(0);
    }
}
