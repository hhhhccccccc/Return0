using cfg;

public class BattleTreasure10011 : BattleTreasureBase
{
    protected override float OnGetSkillWellyRate(int skillGuid)
    {
        var skill = Subject.GetSkill();
        if (skill != null)
        {
            if (skill.Target.CheckVariety(HeroVariety.Weird))
            {
                return GetParamFloat(0);
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
                return GetParamFloat(1);
            }
        }

        return 0;
    }
}
