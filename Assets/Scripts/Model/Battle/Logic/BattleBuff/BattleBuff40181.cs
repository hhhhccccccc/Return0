using cfg;

public class BattleBuff40181 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        DoHealHp(Subject, GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr, BattleSource.Item);
    }
}
