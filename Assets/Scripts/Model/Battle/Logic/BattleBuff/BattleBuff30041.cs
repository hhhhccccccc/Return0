using cfg;
using Zenject;

public class BattleBuff30041 : BattleBuffBase
{
    protected override void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (damageType == DamageType.Direct)
        {
            var attacker = BattleManager.GetUnit(attackID);
            DoReduceHp(attacker, GetConfigParamFloat(0) + GetConfigParamFloat(1) * attacker.Gr, DamageType.InDirect, Subject, BattleSource.Buff);
        }
    }
}
