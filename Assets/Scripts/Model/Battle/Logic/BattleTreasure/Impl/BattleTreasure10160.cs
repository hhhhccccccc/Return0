using cfg;

//todo 表现
public class BattleTreasure10160 : BattleTreasureBase
{
    protected override void OnBeDamage(DamageType damageType)
    {
        if (damageType == DamageType.Direct)
        {
            var buffID = ConfigHelper.GetRandomMedicineID();
            BattleBuffManager.AddBuff(Subject, buffID, Subject, GetConfigParamInt(0));
        }
    }
}