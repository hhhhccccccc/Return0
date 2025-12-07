using System;
using cfg;
using Zenject;

public class BattleBuff40191 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeProperty(BattlePropertyType.Hp, Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr, BattleSource.Item);
    }
}
