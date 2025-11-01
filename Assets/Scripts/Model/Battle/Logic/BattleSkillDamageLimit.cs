using cfg;

public enum BattleSkillDamageLimitType
{
    None = 0,
    Min = 1,
    Max = 2
}

public class BattleSkillDamageLimit : IModel
{
    private static int GlobalGuid = 0;
    public int Guid;
    public SkillType SkillType;
    public BattleSkillDamageLimitType LimitType;
    public float BaseValue;
    
    public void AllocGuid()
    {
        GlobalGuid++;
        Guid = GlobalGuid;
    }

    public void Recycle()
    {
        Guid = 0;
        SkillType = SkillType.None;
        LimitType = BattleSkillDamageLimitType.None;
        BaseValue = 0;
    }
}
