using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10041 : BattleBuffBase
{
    protected override int OnGetChangeActionWheel()
    {
        return LayerCount * Config.ParamEx[0].ToInt();
    }
}
