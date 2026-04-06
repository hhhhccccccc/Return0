using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20291 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        Register<UnitBeHitEventModel>(OnBeHit);
    }

    private void OnBeHit(UnitBeHitEventModel model)
    {
        //直接伤害才能触发共生
        if (model.DamageType != DamageType.Direct)
        {
            return;
        }
        
        //无法生效异常buff
        if (Subject.HasBuffMechanism(BuffMechanism.NotEffectAbnormalBuff))
        {
            return;
        }
        
        if (SpellCaster != null)
        {
            if (SpellCaster.EntityID == model.HitID)
            {
                DoReduceHp(Subject, model.DamageValue * GetConfigParamFloat(0), DamageType.InDirect, BattleManager.GetUnit(model.HitID), BattleSource.Buff);
            }
        }
    }
}
