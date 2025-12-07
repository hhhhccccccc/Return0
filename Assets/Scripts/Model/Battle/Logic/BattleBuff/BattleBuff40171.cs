using System;
using cfg;
using Zenject;

public class BattleBuff40171 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyRight, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
