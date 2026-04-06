using cfg;

public class BattleBuff20331 : BattleBuffBase
{
    protected override void OnBuffRemove()
    {
        var reduceHp = (GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr);
        DoReduceHp(Subject, reduceHp, DamageType.InDirect, BattleManager.GetUnit(SpellCaster.EntityID), BattleSource.Buff);
    }
}
