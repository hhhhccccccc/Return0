
using cfg;

public class BattleTreasure10002 : BattleTreasureBase
{
    private bool CanTrigger { get; set; }
    private bool InTrigger { get; set; }
    private int CD { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        CanTrigger = true;
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
                    CanTrigger = false;
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
                    if (CD <= 0)
                    {
                        CanTrigger = true;
                    }
                }
            }
        }
    }

    protected override float OnGetSkillWellyRate(int skillGuid)
    {
        if (InTrigger)
        {
            return GetParamFloat(2);
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
            if (CD <= 0)
            {
                CanTrigger = true;
            }
        }
        
        InTrigger = false;
    }
}
