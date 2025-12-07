using cfg;

public class BattleBuff90010 : BattleBuffBase
{
    protected override float OnGetAddWellyRate(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        var skillType = BattleUtil.GetSkillTypeBySkillID(s);
        if (skillType == SkillType.ArtKilling)
        {
            return Config.ParamEx[0];
        }

        return 0;
    }
}
