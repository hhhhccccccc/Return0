using System;
using cfg;
using Zenject;

public class BattleBuff40121 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        Subject.AddRandomKey(Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
