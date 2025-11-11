using cfg;

public class BattleBuff20331 : BattleBuffBase
{
    protected override void OnBuffRemove()
    {
        var reduceHp = (Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr);
        Subject.ReduceHp(reduceHp, DamageType.InDirect, SpellCaster.EntityID, source: BattleSource.Buff);
    }
}
