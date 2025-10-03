using cfg;
public class DamageParamModel : MomentParamModel, IRecycle
{
    public BattleClashType BattleClashType;
    public int AttackID;
    public int HitID;
    public int AttackSkillID;
    public int HitSkillID;
    public SkillType AttackSkillType;
    public SkillType HitSkillType;
    public DamageType AttackDamageType;
    public DamageType HitDamageType;
    public BattleSource AttackSource;
    public BattleSource HitSource;
    public bool ClashWin;
    public float AttackDamageValue;
    public float HitDamageValue;
    public float AttackHpValue;
    public float HitHpValue;
    public float AttackShieldValue;
    public float HitShieldValue;
    
    public void Recycle()
    {
        BattleClashType = BattleClashType.None;
        AttackID = 0;
        HitID = 0;
        AttackSkillID = 0;
        HitSkillID = 0;
        AttackSkillType = SkillType.None;
        HitSkillType = SkillType.None;
        AttackDamageType = DamageType.None;
        HitDamageType = DamageType.None;
        AttackSource = BattleSource.None;
        HitSource = BattleSource.None;
        ClashWin = false;
        AttackDamageValue = 0;
        HitDamageValue = 0;
        AttackHpValue = 0;
        HitHpValue = 0;
        AttackShieldValue = 0;
        HitShieldValue = 0;
    }
}
