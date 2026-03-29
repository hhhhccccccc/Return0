using cfg;

public class BattleBuffChangeProperty : BattleBuffBase
{
    private int PropertyID;
    private float PropertyValue;
    protected override void OnBuffStart()
    {
        base.OnBuffStart();
        PropertyID = Config.ParamEx[0].ToInt();
        if (ParamList.Count > 0)
        {
            PropertyValue = ParamList[0];
        }

        if (PropertyValue > 0)
        {
            Subject.ChangeProperty((BattlePropertyType)PropertyID, PropertyValue);
        }
    }

    protected override void OnBuffRemove()
    {
        if (PropertyValue > 0)
        {
            Subject.ChangeProperty((BattlePropertyType)PropertyID, -PropertyValue);
        }
    }

    protected override void OnBuffRecycle()
    {
        PropertyID = 0;
        PropertyValue = 0;
    }
}
