using Zenject;

public class BattleMomentCondition_NotUseSomeKeySkill : BattleMomentCondition
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    protected override bool OnCondition()
    {
        var config = ConfigManager.GetBattleSkillConfig(SkillID);
        return config.NeedKey.Count != Config.ParamList[0].ToInt();
    }
}