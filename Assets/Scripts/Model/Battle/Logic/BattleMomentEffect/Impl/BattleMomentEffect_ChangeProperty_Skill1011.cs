using cfg;

public class BattleMomentEffect_ChangeProperty_Skill1011 : BattleMomentEffect_ChangeProperty
{
    private const int SkillID = 1011;
    private const float ReduceValue = 5f;
    protected override float GetChangePropertyValue()
    {
        var preUseData = Subject.GetSkillPreUseData(SkillID);
        var useCount = preUseData?.UseCount ?? 0;
        return Config.ParamList[2] - useCount * ReduceValue;
    }
}