using cfg;
public class DamageParamModel : MomentParamModel, IRecycle
{
    public BattleClashType BattleClashType;
    public SkillType AttackSkillType;
    public SkillType HitSkillType;
    public DamageType AttackDamageType;
    public DamageType HitDamageType;
    public BattleSource AttackSource;
    public BattleSource HitSource;
    public float AttackDamageValue;
    public float HitDamageValue;
    public float AttackHpValue;
    public float HitHpValue;
    public float AttackShieldValue;
    public float HitShieldValue;
    
    public void Recycle()
    {
        BattleClashType = BattleClashType.None;
        AttackSkillType = SkillType.None;
        HitSkillType = SkillType.None;
        AttackDamageType = DamageType.None;
        HitDamageType = DamageType.None;
        AttackSource = BattleSource.None;
        HitSource = BattleSource.None;
        AttackDamageValue = 0;
        HitDamageValue = 0;
        AttackHpValue = 0;
        HitHpValue = 0;
        AttackShieldValue = 0;
        HitShieldValue = 0;
    }
}
