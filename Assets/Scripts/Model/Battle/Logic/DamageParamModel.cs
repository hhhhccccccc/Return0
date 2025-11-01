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
    public bool AttackClashWin;
    public bool HitClashWin;
    public float AttackTruthDamageValue;
    public float HitTruthDamageValue;
    public float AttackHpValue;
    public float HitHpValue;
    public float AttackShieldValue;
    public float HitShieldValue;
    public float AttackArmorValue;
    public float HitArmorValue;
    
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
        AttackClashWin = false;
        HitClashWin = false;
        AttackTruthDamageValue = 0;
        HitTruthDamageValue = 0;
        AttackHpValue = 0;
        HitHpValue = 0;
        AttackShieldValue = 0;
        HitShieldValue = 0;
        AttackArmorValue = 0;
        HitArmorValue = 0;
    }
}
