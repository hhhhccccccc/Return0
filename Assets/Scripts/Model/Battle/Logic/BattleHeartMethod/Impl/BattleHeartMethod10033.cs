using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10033 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }
    private bool CanTrigger { get; set; }
    private int Round { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        InTrigger = false;
        CanTrigger = true;
    }

    public override bool CheckReCalculateDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger)
        {
            return false;
        }
        
        //附带了雨割 不是扣除体上限才行
        if (paramModel is DamageParamModel model)
        {
            if ((Subject.GetProperty(BattlePropertyType.Hp) - model.GetOtherAttackHpValue(Subject.EntityID)) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
                GetConfigParamFloat(0) && !model.GetOtherDamageReduceMaxHp(Subject.EntityID))
            {
                InTrigger = true;
                CanTrigger = false;
                return true;
            }
        }

        return false;
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!InTrigger)
        {
            return 0;
        }

        if (propertyType == BattlePropertyType.DefendPct)
        {
            return GetConfigParamFloat(1);
        }

        if (propertyType == BattlePropertyType.BreakPct)
        {
            return GetConfigParamFloat(2);
        }

        return 0;
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        InTrigger = false;
    }

    protected override void OnHeartMethodRecycle()
    {
        InTrigger = false;
        CanTrigger = false;
        Round = 0;
    }
}