using System;
using cfg;
using Zenject;

public class BattleBuff40041 : BattleBuffBase
{
    protected override void OnSelfActionWheelStart()
    {
        Subject.ChangeProperty(BattlePropertyType.GangQi, Config.ParamEx[0] * LayerCount);
    }
}
