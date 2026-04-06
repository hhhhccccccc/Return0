using cfg;

public class BattleHeartMethod10025 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        var buffID = Util.GetRandomBool() ? GameConst.Battle.BuffXunSu : GameConst.Battle.BuffHuanSu;
        DoAddBuff(Subject, buffID, Subject, GetConfigParamInt(0), null, BattleMomentType.RoundStart);
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.SpeedInt)
        {
            return GetConfigParamFloat(1) + GetConfigParamFloat(2) * Subject.Gr;
        }

        return 0;
    }
}