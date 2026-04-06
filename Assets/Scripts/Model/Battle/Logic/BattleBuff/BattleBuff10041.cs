using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10041 : BattleBuffBase
{
    /// <summary>
    /// 每层使行动加快1息
    /// </summary>
    /// <returns></returns>
    protected override int OnGetChangeActionWheel()
    {
        return LayerCount * GetConfigParamInt(0);
    }
}
