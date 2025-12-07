using System;
using cfg;
using Zenject;

public class BattleBuff40151 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyDown, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
