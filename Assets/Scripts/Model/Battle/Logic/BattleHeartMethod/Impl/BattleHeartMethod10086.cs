using cfg;

//todo 表现

public class BattleHeartMethod10086 : BattleHeartMethodBase
{
    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (propType == BattlePropertyType.XuanQi)
        {
            var now = Subject.GetProperty(BattlePropertyType.XuanQi);
            if (now <= GetParamFloat(0))
            {
                Subject.SetProperty(BattlePropertyType.XuanQi, GetParamFloat(0));
            }
        }
    }
}