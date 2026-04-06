using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20231 : BattleBuffBase
{
    //todo 每次获得该状态扣除层数*2%的当前命
    public override int AddLayerCount(int layerCount)
    {
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var reduceHp = GetConfigParamFloat(0) * LayerCount * hp;
        DoReduceHp(Subject, reduceHp, DamageType.InDirect, SpellCaster, source: BattleSource.Buff);
        return LayerCount;
    }
}
