using cfg;

public class BattleBuff30391 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.TechPct)
        {
            if (LayerCount > Config.ParamEx[2].ToInt())
            {
                return Config.ParamEx[0] * 2;
            }
            return Config.ParamEx[0];
        }
        
        if (propertyType == BattlePropertyType.BreakPct)
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
        if (Subject.TransformState != BattleUnitTransformState.Zu && LayerCount > Config.ParamEx[2].ToInt())
        {
            Subject.SetTransformState(BattleUnitTransformState.Zu);
        }
        
        if (Subject.TransformState != BattleUnitTransformState.None && LayerCount <= Config.ParamEx[2].ToInt())
        {
            Subject.SetTransformState(BattleUnitTransformState.None);
        }
    }
}
