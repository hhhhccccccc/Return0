using cfg;

public class BattleBuff30371 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.PowerPct)
        {
            if (LayerCount > GetConfigParamInt(2))
            {
                return GetConfigParamFloat(0) * 2;
            }
            return GetConfigParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.DefendPct)
        {
            if (LayerCount > GetConfigParamInt(2))
            {
                return GetConfigParamFloat(1) * 2;
            }
            return GetConfigParamFloat(1);
        }

        return 0;
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (Subject.TransformState != BattleUnitTransformState.Shou && LayerCount > GetConfigParamInt(2))
        {
            DoSetTransformState(Subject, BattleUnitTransformState.Shou);
        }
        
        if (Subject.TransformState != BattleUnitTransformState.None && LayerCount <= GetConfigParamInt(2))
        {
            DoSetTransformState(Subject, BattleUnitTransformState.None);
        }
    }
}
