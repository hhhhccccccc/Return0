using cfg;

public class BattleBuff30391 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TechPct)
        {
            if (LayerCount > GetConfigParamInt(2))
            {
                return GetConfigParamFloat(0) * 2;
            }
            return GetConfigParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.BreakPct)
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
        if (Subject.TransformState != BattleUnitTransformState.Zu && LayerCount > Config.ParamEx[2].ToInt())
        {
            DoSetTransformState(Subject, BattleUnitTransformState.Zu);
        }
        
        if (Subject.TransformState != BattleUnitTransformState.None && LayerCount <= Config.ParamEx[2].ToInt())
        {
            DoSetTransformState(Subject, BattleUnitTransformState.None);
        }
    }
}
