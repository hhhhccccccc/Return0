using cfg;

public class BattleTreasure10011 : BattleTreasureBase
{
    private float SkillWelly => GetConfigParamFloat(0);
    private float SkillRate => GetConfigParamFloat(1);
    protected override float OnGetSkillWellyRate(int skillGuid)
    {
        var skill = Subject.GetSkill();
        if (skill != null)
        {
            if (skill.Target.CheckVariety(HeroVariety.Weird))
            {
                EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
                return SkillWelly;
            }
        }

        return 0;
    }

    protected override float OnGetSkillDamageRate(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            if (target.CheckVariety(HeroVariety.Weird))
            {
                EnqueueViewModel(Subject.EntityID, MomentViewType.AddRate, SkillRate);
                return SkillRate;
            }
        }

        return 0;
    }
}
