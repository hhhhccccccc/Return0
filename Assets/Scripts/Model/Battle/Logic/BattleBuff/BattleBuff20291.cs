using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20291 : BattleBuffBase
{
    [Inject] private IMessageManager MessageManager { get; set; }
    private float Pct;
    private IDisposable RegisterEvent;
    protected override void OnStart()
    {
        Pct = Config.ParamEx[0];
        Register<UnitBeHitEventModel>(OnBeHit);
        base.OnStart();
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
                Subject.ReduceHp(model.DamageValue * Pct, DamageType.InDirect, model.HitID, false, BattleSource.Buff);
            }
        }
    }
}
