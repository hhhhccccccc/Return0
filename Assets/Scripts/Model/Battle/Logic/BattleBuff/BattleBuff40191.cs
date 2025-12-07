using cfg;

public class BattleBuff40191 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeProperty(BattlePropertyType.Hp, Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr, BattleSource.Item);
    }
}
