using cfg;

public class BattleMomentEffect_ChangeProperty_Skill1010 : BattleMomentEffect_ChangeProperty
{
    private const int SkillID = 1010;
    private const float ReduceValue = 5f;
    protected override float GetChangePropertyValue()
    {
        var useCount = Target.PreUseSkillDataManager.GetSkillUseCount(SkillID);
        return Config.ParamList[2] - useCount * ReduceValue;
    }
}