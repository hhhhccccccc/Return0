using System;
using cfg;
using Zenject;

public class BattleBuff40161 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyLeft, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
