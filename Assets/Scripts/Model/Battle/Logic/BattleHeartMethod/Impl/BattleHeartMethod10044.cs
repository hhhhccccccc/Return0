using cfg;

public class BattleHeartMethod10044 : BattleHeartMethodBase
{
    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(changeHp * GetConfigParamFloat(0)), null, BattleMomentType.None);
    }
}