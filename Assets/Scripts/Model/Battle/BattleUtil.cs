
public static class BattleUtil
{
    public static SkillType GetSkillTypeBySkillID(int skillID)
    {
        return skillID switch
        {
            1 => SkillType.MartialArts,
            2 => SkillType.KillingStyle,
            3 => SkillType.TechniqueImperialStyle,
            4 => SkillType.SpellFormula,
            _ => SkillType.None
        };
    }
    
    public static bool SkillIsKillingStyle(SkillType skillType)
    {
        return skillType == SkillType.KillingStyle || skillType == SkillType.MartialArts;
    }
}
