using cfg;

//todo 表现
public class BattleHeartMethod10111 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Subject.AddNotRecoverGangQiNatural(1);
        Subject.AddNotRecoverXuanQiNatural(1);
    }

    public override void RoundStart()
    {
        var keyCount = Subject.GetAllKeyCount();
        var maxKeyCount = Subject.GetKeyPropertyMax();
        var delta = maxKeyCount - keyCount;
        if (delta > 0)
        {
            var gangQiPct = GetMomentProperty(BattlePropertyType.GangQi) / GetMomentProperty(BattlePropertyType.MaxGangQi);
            var xuanQiPct = GetMomentProperty(BattlePropertyType.XuanQi) / GetMomentProperty(BattlePropertyType.MaxXuanQi);
            var single = GetConfigParamFloat(0);
            if (gangQiPct >= xuanQiPct)
            {
                var cost = GetMomentProperty(BattlePropertyType.MaxGangQi) * single * delta;
                Subject.ChangeProperty(BattlePropertyType.GangQi, cost, BattleSource.HeartMethod);
            }
            else
            {
                var cost = GetMomentProperty(BattlePropertyType.MaxXuanQi) * single * delta;
                Subject.ChangeProperty(BattlePropertyType.XuanQi, cost, BattleSource.HeartMethod);
            }
        }
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (GetMomentProperty(BattlePropertyType.GangQi) <= 0 && GetMomentProperty(BattlePropertyType.XuanQi) <= 0)
        {
            Subject.SetBreak(true);
        }
    }
}