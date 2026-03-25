using cfg;

// todo 表现
public class BattleHeartMethod10044 : BattleHeartMethodBase
{
    public override void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(reduceHp * GetParamFloat(0)));
    }
}