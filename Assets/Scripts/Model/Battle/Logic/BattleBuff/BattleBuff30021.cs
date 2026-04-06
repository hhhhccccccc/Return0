using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30021 : BattleBuffBase
{
    protected override int OnGetChangeActionWheel()
    {
        return GetConfigParamInt(0);
    }
}
