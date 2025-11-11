using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30021 : BattleBuffBase
{
    protected override int OnGetChangeActionWheel()
    {
        return Config.ParamEx[0].ToInt();
    }
}
