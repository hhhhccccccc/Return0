using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10034 : BattleHeartMethodBase
{
    /// <summary>
    /// 在攻击的时候会调用一次 确保是上次的
    /// </summary>
    private bool LastInTrigger { get; set; }
    private bool InTrigger { get; set; }

    public override void HpChanged()
    {
        LastInTrigger = InTrigger;
        InTrigger = Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <= GetParamFloat(0);
    }

    public override void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        if (!LastInTrigger)
        {
            return;
        }
        
        var attacker = BattleManager.GetUnit(attackID);
        var skill = attacker.GetSkill();
        if (skill != null && skill.SkillIsKillingStyle() && damageType == DamageType.Direct)
        {
            //todo 
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10161, Subject, GetParamInt(1));
        }
    }

    protected override void OnRecycle()
    {
        LastInTrigger = false;
        InTrigger = false;
    }
}