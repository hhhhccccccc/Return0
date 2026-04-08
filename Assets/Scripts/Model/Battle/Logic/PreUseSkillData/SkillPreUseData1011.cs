
public class SkillPreUseData1011 : SkillPreUseDataBase
{
    public override float GetXuanQiCost()
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(SkillID);
        return skillConfig.XuanQiCost - 2 * UseCount;
    }
}
