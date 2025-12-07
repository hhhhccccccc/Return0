using System;
using cfg;
using Zenject;

public class BattleBuff40061 : BattleBuffBase
{
    protected override void OnSelfActionWheelStart()
    {
        Subject.ChangeProperty(BattlePropertyType.XuanQi, Config.ParamEx[0]);
    }
}
