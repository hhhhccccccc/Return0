using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10055 : BattleHeartMethodBase
{
    private int FactionID => GetParamInt(0);
    
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRedPct)
        {
            if (model != null)
            {
                if (model.SourceType == GetPropertySourceType.GetSkillCostView
                    || model.SourceType == GetPropertySourceType.GetSkillCostCheck
                    || model.SourceType == GetPropertySourceType.GetSkillCostLogic)
                {
                    var (s, v) = Util.UnCombSkillGuid(model.TypeID);
                    if (FactionID == BattleUtil.GetSkillFactionID(s))
                    {
                        return GetParamFloat(1);
                    }
                }
            }
        }
        
        if (propertyType == BattlePropertyType.XuanQiRedPct)
        {
            if (model != null)
            {
                if (model.SourceType == GetPropertySourceType.GetSkillCostView
                    || model.SourceType == GetPropertySourceType.GetSkillCostCheck
                    || model.SourceType == GetPropertySourceType.GetSkillCostLogic)
                {
                    var (s, v) = Util.UnCombSkillGuid(model.TypeID);
                    if (FactionID == BattleUtil.GetSkillFactionID(s))
                    {
                        return GetParamFloat(2);
                    }
                }
            }
        }

        return 0;
    }
}