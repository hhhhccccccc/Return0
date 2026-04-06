using cfg;

public class BattleBuff30381 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.CleverPct)
        {
            if (LayerCount > GetConfigParamInt(2))
            {
                return GetConfigParamFloat(0) * 2;
            }
            return GetConfigParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.SpeedPct)
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
        if (Subject.TransformState != BattleUnitTransformState.Qin && LayerCount > Config.ParamEx[2].ToInt())
        {
            DoSetTransformState(Subject, BattleUnitTransformState.Qin);
        }
        
        if (Subject.TransformState != BattleUnitTransformState.None && LayerCount <= Config.ParamEx[2].ToInt())
        {
            DoSetTransformState(Subject, BattleUnitTransformState.None);
        }
    }
}
