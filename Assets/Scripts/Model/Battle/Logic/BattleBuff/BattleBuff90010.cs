using cfg;

public class BattleBuff90010 : BattleBuffBase
{
    protected override float OnGetWellyRateEx(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        var skillType = BattleUtil.GetSkillTypeBySkillID(s);
        if (skillType == SkillType.ArtKilling)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}
