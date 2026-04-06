using cfg;

public class BattleTreasure10011 : BattleTreasureBase
{
    private float WellyRate => GetConfigParamFloat(0);
    private float DamagePct => GetConfigParamFloat(1);
    protected override float OnGetSkillWellyRate(int skillGuid)
    {
        var skill = Subject.GetSkill();
        if (skill != null)
        {
            if (skill.Target.CheckVariety(HeroVariety.Weird))
            {
                return WellyRate;
            }
        }

        return 0;
    }

    protected override float OnAddDamagePct(MomentParamModel paramModel)
    {
        var other = GetOtherUnit(paramModel);
        if (other.CheckVariety(HeroVariety.Weird))
        {
            return DamagePct;
        }

        return 0;
    }
}
