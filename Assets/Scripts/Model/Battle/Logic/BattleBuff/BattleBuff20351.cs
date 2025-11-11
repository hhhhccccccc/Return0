using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20351 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }

    public override void ClearLayerCount()
    {
        var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
        ReduceLayerCount(reduceCount);
    }

    protected override void OnBeAttack(float reduceHp, DamageType damageType, int attackID)
    {
        if (damageType == DamageType.Direct)
        {
            var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
            ReduceLayerCount(reduceCount);
        }
    }
}
