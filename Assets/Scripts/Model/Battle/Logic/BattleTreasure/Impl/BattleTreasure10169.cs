using cfg;

public class BattleTreasure10169 : BattleTreasureBase
{
    private bool CanTrigger { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        CanTrigger = true;
    }
    protected override bool OnCanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger)
        {
            return false;
        } 
        if (paramModel is DamageParamModel model)
        {
            var damageHp = model.GetOtherAttackHpValue(Subject.EntityID);
            if (Subject.GetProperty(BattlePropertyType.Hp) <= damageHp)
            {
                CanTrigger = false;
                return true;
            }
        }
        return false;
    }

    protected override void OnTreasureRecycle()
    {
        CanTrigger = false;
    }
}