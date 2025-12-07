using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10151 : BattleBuffBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        Subject.AddRandomKey(Config.ParamEx[0].ToInt(), ChangeKeyReason.BuffEffect);
    }
}
