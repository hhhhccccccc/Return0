using cfg;
public class BattleHeartMethod10086 : BattleHeartMethodBase
{
    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (propType == BattlePropertyType.XuanQi)
        {
            var now = Subject.GetProperty(BattlePropertyType.XuanQi);
            if (now <= GetConfigParamFloat(0))
            {
                DoSetProperty(Subject, BattlePropertyType.XuanQi, GetConfigParamFloat(0), BattleSource.HeartMethod);
            }
        }
    }
}