using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10080 : BattleHeartMethodBase
{
    private float DefendPct { get; set; }
    private float BreakPct { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        DefendPct = GetConfigParamFloat(0);
        BreakPct = GetConfigParamFloat(1);
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (model is { SourceType: GetPropertySourceType.ReceiveSkillDamage })
        {
            var attacker = BattleManager.GetUnit(model.AttackerID);
            if (attacker != null)
            {
                var buffCount = attacker.GetBuffCountByID(GameConst.Battle.BuffYaoDuQinShi);
                if (propertyType == BattlePropertyType.DefendPct)
                {
                    return DefendPct - buffCount * GetConfigParamFloat(2);
                }
                if (propertyType == BattlePropertyType.BreakPct)
                {
                    return BreakPct - buffCount * GetConfigParamFloat(3);
                }
            }
        }
        else
        {
            if (propertyType == BattlePropertyType.DefendPct)
            {
                return DefendPct;
            }
            if (propertyType == BattlePropertyType.BreakPct)
            {
                return BreakPct;
            }
        }

        return 0;
    }

    protected override void OnHeartMethodRecycle()
    {
        DefendPct = 0;
        BreakPct = 0;
    }
}