using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20011 : BattleBuffBase
{
    protected override int OnGetChangeActionWheel()
    {
        return LayerCount * Config.ParamEx[0].ToInt();
    }
}
