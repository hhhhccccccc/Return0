using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10151 : BattleBuffBase
{
    /// <summary>
    /// 行动后获得1个随机的键
    /// </summary>
    /// <param name="paramModel"></param>
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, GetConfigParamInt(0), ChangeKeyReason.BuffEffect);
    }
}
