using cfg;

public class BattleHeartMethod10056 : BattleHeartMethodBase
{
    public override void AfterUnitInit()
    {
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var check = maxHp * GetConfigParamFloat(1);
        if (hp >= check)
        {
            DoSetHp(Subject, check, Subject, BattleSource.HeartMethod);
        }
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.MaxHpPct)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}