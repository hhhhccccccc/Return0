using System;
using cfg;
using Zenject;

public class BattleBuff40071 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        Subject.ChangeProperty(BattlePropertyType.GangQi, Config.ParamEx[0] * LayerCount);
    }
}
