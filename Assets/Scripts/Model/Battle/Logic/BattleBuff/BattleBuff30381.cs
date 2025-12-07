using cfg;

public class BattleBuff30381 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.CleverPct)
        {
            if (LayerCount > Config.ParamEx[2].ToInt())
            {
                return Config.ParamEx[0] * 2;
            }
            return Config.ParamEx[0];
        }
        
        if (propertyType == BattlePropertyType.SpeedPct)
        {
            if (LayerCount > Config.ParamEx[2].ToInt())
            {
                return Config.ParamEx[1] * 2;
            }
            return Config.ParamEx[1];
        }

        return 0;
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (Subject.TransformState != BattleUnitTransformState.Qin && LayerCount > Config.ParamEx[2].ToInt())
        {
            Subject.SetTransformState(BattleUnitTransformState.Qin);
        }
        
        if (Subject.TransformState != BattleUnitTransformState.None && LayerCount <= Config.ParamEx[2].ToInt())
        {
            Subject.SetTransformState(BattleUnitTransformState.None);
        }
    }
}
