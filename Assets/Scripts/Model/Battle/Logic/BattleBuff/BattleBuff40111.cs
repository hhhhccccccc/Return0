using System;
using cfg;
using Zenject;

public class BattleBuff40111 : BattleBuffBase
{
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        Subject.AddRandomKey(Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
