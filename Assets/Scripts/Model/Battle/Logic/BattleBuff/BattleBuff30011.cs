using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30011 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        if (LayerCount >= Config.ParamEx[0].ToInt())
        {
            Subject.ChangeProperty(BattlePropertyType.GangQi, Config.ParamEx[1], BattleSource.Buff);
            ClearLayerCount();
        }
    }
}
