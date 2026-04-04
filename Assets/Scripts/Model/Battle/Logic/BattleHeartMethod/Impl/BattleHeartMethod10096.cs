using cfg;


public class BattleHeartMethod10096 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        var skill = Subject.GetSkill();
        if (propertyType == BattlePropertyType.DefendPct && skill.IsInAction)
        {
            return GetConfigParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.BreakPct && skill.IsInAction)
        {
            return GetConfigParamFloat(1);
        }

        return 0;
    }
}