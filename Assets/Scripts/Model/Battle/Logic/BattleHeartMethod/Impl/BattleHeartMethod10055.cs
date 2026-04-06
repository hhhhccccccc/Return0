using cfg;

public class BattleHeartMethod10055 : BattleHeartMethodBase
{
    private int FactionID => GetConfigParamInt(0);
    private float GangQiPct => GetConfigParamFloat(1);
    private float XuanQiPct => GetConfigParamFloat(2);
    
    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
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
                        return GangQiPct;
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
                        return XuanQiPct;
                    }
                }
            }
        }

        return 0;
    }
}