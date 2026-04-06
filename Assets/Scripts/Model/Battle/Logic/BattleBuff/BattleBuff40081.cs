using cfg;

public class BattleBuff40081 : BattleBuffPotion
{
    protected override void OnRoundStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, GetConfigParamFloat(0), BattleSource.Item);
    }
}
