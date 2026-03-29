using cfg;

// todo 表现
public class BattleHeartMethod10044 : BattleHeartMethodBase
{
    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(changeHp * GetParamFloat(0)));
    }
}