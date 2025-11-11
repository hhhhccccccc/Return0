using cfg;
using Zenject;

public class BattleBuff30041 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    protected override void OnBeAttack(float reduceHp, DamageType damageType, int attackID)
    {
        if (damageType == DamageType.Direct)
        {
            var attacker = BattleManager.GetUnit(attackID);
            attacker.ReduceHp(Config.ParamEx[0] + Config.ParamEx[1] * attacker.Gr, DamageType.InDirect,
                Subject.EntityID, source: BattleSource.Buff);
        }
    }
}
