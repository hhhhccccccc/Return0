
using cfg;

public class BattleTreasure10002 : BattleTreasureBase
{
    private float SkillWelly => GetParamFloat(2);
    private bool CanTrigger => CD <= 0;
    private bool InTrigger { get; set; }
    private int CD { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        CD = 0;
        InTrigger = false;
    }

    protected override void OnBeforeClash(MomentParamModel paramModel)
    {
        if (CanTrigger)
        {
            if (paramModel is DamageParamModel model)
            {
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                var selfDamageRate = Subject.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                var otherDamageRate = target.GetSkillDamageWelly(SkillDataGetType.DamageCurr);
                if (otherDamageRate <= GetParamFloat(0) && otherDamageRate >= selfDamageRate && otherDamageRate - selfDamageRate <= GetParamFloat(1))
                {
                    InTrigger = true;
                    CD = GetParamInt(3);
                }
            }
        }
    }

    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (!model.GetSelfClashState(Subject.EntityID))
            {
                if (CD > 0)
                {
                    CD--;
                }
            }
        }
    }

    protected override float OnGetSkillWellyRate(int skillGuid)
    {
        if (InTrigger)
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
            return skillGuid;
        }

        return 0;
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        InTrigger = false;
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        if (CD > 0)
        {
            CD--;
        }
        
        InTrigger = false;
    }
}
