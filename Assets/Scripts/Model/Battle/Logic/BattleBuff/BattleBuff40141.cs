using System;
using cfg;
using Zenject;

public class BattleBuff40141 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyUp, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
