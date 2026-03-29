using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10034 : BattleHeartMethodBase
{
    /// <summary>
    /// 在攻击的时候会调用一次 确保是上次的
    /// </summary>
    private bool CanTrigger { get; set; }
    public override void BeforeChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        CanTrigger = Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <= GetParamFloat(0) && !isReduceHpMax && damageType == DamageType.Direct;
    }

    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (!isReduce)
        {
            return;
        }
                
        if (damageType != DamageType.Direct)
        {
            return;
        }
        
        if (isReduceHpMax)
        {
            return;
        }
        
        if (!CanTrigger)
        {
            return;
        }
        
        var attacker = BattleManager.GetUnit(attackID);
        var skill = attacker.GetSkill();
        if (skill != null && skill.SkillIsKillingStyle() && damageType == DamageType.Direct)
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffNiXing, Subject, GetParamInt(1));
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}