using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20231 : BattleBuffBase
{
    public override void AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        var reduceHp = Config.ParamEx[0] * LayerCount;
        Subject.ReduceHp(reduceHp, DamageType.InDirect, SpellCaster.EntityID, source: BattleSource.Buff);
    }
}
