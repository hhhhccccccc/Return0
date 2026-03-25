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
                EnqueueViewModel(Subject.EntityID, MomentViewType.IgnoreSkillDirectDamage);
                return true;
            }
        }
        return false;
    }

    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}