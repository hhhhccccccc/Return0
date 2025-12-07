using cfg;
using Zenject;

public class DamageParamModel : MomentParamModel, IRecycle
{
    public BattleClashType BattleClashType { get; set; }
    public int AttackID { get; set; }
    public int HitID { get; set; }
    public int AttackSkillID { get; set; }
    public int HitSkillID { get; set; }
    public int AttackVariantID { get; set; }
    public int HitVariantID { get; set; }
    public SkillType AttackSkillType { get; set; }
    public SkillType HitSkillType { get; set; }
    public DamageType AttackDamageType { get; set; }
    public DamageType HitDamageType { get; set; }
    public BattleSource AttackSource { get; set; }
    public BattleSource HitSource { get; set; }
    public float AttackFinalDamageRate { get; set; }
    public float HitFinalDamageRate { get; set; }
    public bool AttackClashWin { get; set; }
    public bool HitClashWin { get; set; }
    public float AttackTruthDamageValue { get; set; }
    public float HitTruthDamageValue { get; set; }
    public float AttackHpValue { get; set; }
    public float HitHpValue { get; set; }
    public float AttackShieldValue { get; set; }
    public float HitShieldValue { get; set; }
    public float AttackArmorValue { get; set; }
    public float HitArmorValue { get; set; }
    
    public void Recycle()
    {
        BattleClashType = BattleClashType.None;
        AttackID = 0;
        HitID = 0;
        AttackSkillID = 0;
        AttackVariantID = 0;
        HitSkillID = 0;
        HitVariantID = 0;
        AttackSkillType = SkillType.None;
        HitSkillType = SkillType.None;
        AttackDamageType = DamageType.None;
        HitDamageType = DamageType.None;
        AttackSource = BattleSource.None;
        HitSource = BattleSource.None;
        AttackClashWin = false;
        HitClashWin = false;
        AttackFinalDamageRate = 0;
        HitFinalDamageRate = 0;
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
