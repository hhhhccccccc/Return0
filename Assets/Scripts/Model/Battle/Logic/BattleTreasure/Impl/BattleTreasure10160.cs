using cfg;

public class BattleTreasure10160 : BattleTreasureBase
{
    protected override void OnBeDamage(DamageType damageType)
    {
        if (damageType == DamageType.Direct)
        {
            var buffID = ConfigHelper.GetRandomMedicineID();
            DoAddBuff(Subject, buffID, Subject, GetConfigParamInt(0), null, BattleMomentType.None);
        }
    }
}