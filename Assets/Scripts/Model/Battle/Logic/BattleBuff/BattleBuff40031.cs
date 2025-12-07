using cfg;

public class BattleBuff40031 : BattleBuffPotion
{
    protected override void OnRoundStart()
    {
        Subject.ChangeProperty(BattlePropertyType.GangQi, Config.ParamEx[0] * LayerCount);
    }
}
