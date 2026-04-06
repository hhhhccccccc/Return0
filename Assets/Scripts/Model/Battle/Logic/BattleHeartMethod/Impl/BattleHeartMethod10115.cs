using cfg;

public class BattleHeartMethod10115 : BattleHeartMethodBase
{
    public override bool CheckCanRecoverNaturalQi(BattlePropertyType propertyType)
    {
        return false;
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (Subject.GetProperty(BattlePropertyType.GangQi) <= 0 && Subject.GetProperty(BattlePropertyType.XuanQi) <= 0)
        {
            DoSetBreak(Subject, true);
        }
    }
}