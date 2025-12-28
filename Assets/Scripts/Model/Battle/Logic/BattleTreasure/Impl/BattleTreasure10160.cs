using cfg;

public class BattleTreasure10160 : BattleTreasureBase
{
    protected override void OnBeDamage(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetOtherDamageType(Subject.EntityID) == DamageType.Direct)
            {
                var buffID = ConfigHelper.GetRandomMedicineID();
                BattleBuffManager.AddBuff(Subject, buffID, Subject, GetParamInt(0));
            }
        }
    }
}