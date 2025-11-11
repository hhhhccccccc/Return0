using Zenject;

public class BattleMomentCondition_NotUseVariantSkill : BattleMomentCondition
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    protected override bool OnCondition()
    {
        return VariantID == 0;
    }
}