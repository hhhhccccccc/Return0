using cfg;

public class BattleBuff40071 : BattleBuffPotion
{
    protected override void OnRoundStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, GetConfigParamFloat(0), BattleSource.Item);
    }
}
