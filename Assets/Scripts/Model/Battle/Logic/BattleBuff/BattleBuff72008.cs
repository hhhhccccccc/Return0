using cfg;

public class BattleBuff72008 : BattleBuffBase
{
    private float PropertyValue;
    protected override void OnBuffStart()
    {
        if (ParamList.Count > 0)
        {
            PropertyValue = ParamList[0];
        }

        if (PropertyValue > 0)
        {
            DoChangeProperty(Subject, BattlePropertyType.PowerInt, PropertyValue, BattleSource.Buff);
        }
    }

    protected override void OnBuffRemove()
    {
        if (PropertyValue > 0)
        {
            DoChangeProperty(Subject, BattlePropertyType.PowerInt, -PropertyValue, BattleSource.Buff);
        }
    }

    protected override void OnBuffRecycle()
    {
        PropertyValue = 0;
    }
}
