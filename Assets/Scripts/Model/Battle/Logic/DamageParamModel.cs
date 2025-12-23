using System.Collections.Generic;
using cfg;
using Zenject;

public class DamageParamModel : MomentParamModel, IRecycle
{
    public BattleClashType BattleClashType { get; set; }

    #region 目标
    public int SelfID { get; set; }
    public int OtherID { get; set; }
    public int GetSelfID(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfID;
        }

        if (entityID == OtherID)
        {
            return OtherID;
        }

        return 0;
    }
    public int GetOtherID(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherID;
        }

        if (entityID == OtherID)
        {
            return SelfID;
        }

        return 0;
    }
    #endregion

    #region 技能ID
    private int SelfSkillID { get; set; }
    private int OtherSkillID { get; set; }
    public int GetSelfSkillID(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfSkillID;
        }

        if (entityID == OtherID)
        {
            return OtherSkillID;
        }

        return 0;
    }
    public int GetOtherSkillID(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherSkillID;
        }

        if (entityID == OtherID)
        {
            return SelfSkillID;
        }

        return 0;
    }
    public void SetSkillID(int entityID, int skillID)
    {
        if (entityID == SelfID)
        {
            SelfSkillID = skillID;
        }

        if (entityID == OtherID)
        {
            OtherSkillID = skillID;
        }
    }
    #endregion

    #region 变式ID
    private int SelfVariantID { get; set; }
    private int OtherVariantID { get; set; }
    public int GetSelfVariantID(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfVariantID;
        }

        if (entityID == OtherID)
        {
            return OtherVariantID;
        }

        return 0;
    }
    public int GetOtherVariantID(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherVariantID;
        }

        if (entityID == OtherID)
        {
            return SelfVariantID;
        }

        return 0;
    }
    public void SetVariantID(int entityID, int skillID)
    {
        if (entityID == SelfID)
        {
            SelfVariantID = skillID;
        }

        if (entityID == OtherID)
        {
            OtherVariantID = skillID;
        }
    }
    #endregion

    #region 技能类型
    private SkillType SelfSkillType { get; set; }
    private SkillType OtherSkillType { get; set; }
    public SkillType GetSelfSkillType(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfSkillType;
        }

        if (entityID == OtherID)
        {
            return OtherSkillType;
        }

        return 0;
    }
    public SkillType GetOtherSkillType(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherSkillType;
        }

        if (entityID == OtherID)
        {
            return SelfSkillType;
        }

        return 0;
    }
    public void SetSkillType(int entityID, SkillType skillType)
    {
        if (entityID == SelfID)
        {
            SelfSkillType = skillType;
        }

        if (entityID == OtherID)
        {
            OtherSkillType = skillType;
        }
    }

    #endregion

    #region 资源消耗

    private float SelfGangQiCost { get; set; }
    private float OtherGangQiCost { get; set; }
    public float GetSelfGangQiCost(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfGangQiCost;
        }

        if (entityID == OtherID)
        {
            return OtherGangQiCost;
        }

        return 0;
    }

    public float GetOtherGangQiCost(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherGangQiCost;
        }

        if (entityID == OtherID)
        {
            return SelfGangQiCost;
        }

        return 0;
    }
    public void SetGangQiCost(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfGangQiCost = value;
        }

        if (entityID == OtherID)
        {
            OtherGangQiCost = value;
        }
    }
    
    private float SelfXuanQiCost { get; set; }
    private float OtherXuanQiCost { get; set; }
    public float GetSelfXuanQiCost(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfXuanQiCost;
        }

        if (entityID == OtherID)
        {
            return OtherXuanQiCost;
        }

        return 0;
    }

    public float GetOtherXuanQiCost(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherXuanQiCost;
        }

        if (entityID == OtherID)
        {
            return SelfXuanQiCost;
        }

        return 0;
    }
    public void SetXuanQiCost(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfXuanQiCost = value;
        }

        if (entityID == OtherID)
        {
            OtherXuanQiCost = value;
        }
    }

    private List<BattleKey> SelfKeyCost { get; set; } = new();
    private List<BattleKey> OtherKeyCost { get; set; } = new();
    public List<BattleKey> GetSelfKeyCost(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfKeyCost;
        }

        if (entityID == OtherID)
        {
            return OtherKeyCost;
        }

        return null;
    }

    public List<BattleKey> GetOtherKeyCost(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherKeyCost;
        }

        if (entityID == OtherID)
        {
            return SelfKeyCost;
        }

        return null;
    }
    public void SetKeyCost(int entityID, List<BattleKey> keyList)
    {
        if (entityID == SelfID)
        {
            SelfKeyCost.AddRange(keyList);
        }

        if (entityID == OtherID)
        {
            OtherKeyCost.AddRange(keyList);
        }
    }
    
    #endregion

    #region 伤害类型
    private DamageType SelfDamageType { get; set; }
    private DamageType OtherDamageType { get; set; }
    public DamageType GetSelfDamageType(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfDamageType;
        }

        if (entityID == OtherID)
        {
            return OtherDamageType;
        }

        return 0;
    }
    public DamageType GetOtherDamageType(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherDamageType;
        }

        if (entityID == OtherID)
        {
            return SelfDamageType;
        }

        return 0;
    }
    public void SetDamageType(int entityID, DamageType damageType)
    {
        if (entityID == SelfID)
        {
            SelfDamageType = damageType;
        }

        if (entityID == OtherID)
        {
            OtherDamageType = damageType;
        }
    }
    #endregion

    #region 源
    private BattleSource SelfSource { get; set; }
    private BattleSource OtherSource { get; set; }
    public BattleSource GetSelfBattleSource(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfSource;
        }

        if (entityID == OtherID)
        {
            return OtherSource;
        }

        return 0;
    }
    public BattleSource GetOtherBattleSource(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherSource;
        }

        if (entityID == OtherID)
        {
            return SelfSource;
        }

        return 0;
    }
    public void SetBattleSource(int entityID, BattleSource sourceType)
    {
        if (entityID == SelfID)
        {
            SelfSource = sourceType;
        }

        if (entityID == OtherID)
        {
            OtherSource = sourceType;
        }
    }
    #endregion

    #region 最终伤害威力
    private float SelfFinalDamageRate { get; set; }
    private float OtherFinalDamageRate { get; set; }
    public float GetSelfFinalDamageRate(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfFinalDamageRate;
        }

        if (entityID == OtherID)
        {
            return OtherFinalDamageRate;
        }

        return 0;
    }
    public float GetOtherFinalDamageRate(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherFinalDamageRate;
        }

        if (entityID == OtherID)
        {
            return SelfFinalDamageRate;
        }

        return 0;
    }
    public void SetFinalDamageRate(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfFinalDamageRate = value;
        }

        if (entityID == OtherID)
        {
            OtherFinalDamageRate = value;
        }
    }
    #endregion

    #region 交锋结果
    private bool SelfClashState { get; set; }
    private bool OtherClashState { get; set; }
    public bool GetSelfClashState(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfClashState;
        }

        if (entityID == OtherID)
        {
            return OtherClashState;
        }

        return false;
    }
    public bool GetOtherClashState(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherClashState;
        }

        if (entityID == OtherID)
        {
            return SelfClashState;
        }

        return false;
    }
    public void SetClashState(int entityID, bool state)
    {
        if (entityID == SelfID)
        {
            SelfClashState = state;
        }

        if (entityID == OtherID)
        {
            OtherClashState = state;
        }
    }
    #endregion

    #region 技能是否释放成功

    private bool SelfUseSuccess { get; set; }
    private bool OtherUseSuccess { get; set; }
    
    public bool GetSelfUseSuccess(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfUseSuccess;
        }

        if (entityID == OtherID)
        {
            return OtherUseSuccess;
        }

        return false;
    }
    public bool GetOtherUseSuccess(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherUseSuccess;
        }

        if (entityID == OtherID)
        {
            return SelfUseSuccess;
        }

        return false;
    }
    public void SetUseSuccess(int entityID, bool state)
    {
        if (entityID == SelfID)
        {
            SelfUseSuccess = state;
        }

        if (entityID == OtherID)
        {
            OtherUseSuccess = state;
        }
    }

    #endregion

    #region 真实伤害
    private float SelfTruthDamageValue { get; set; }
    private float OtherTruthDamageValue { get; set; }
    public float GetSelfTruthDamageValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfTruthDamageValue;
        }

        if (entityID == OtherID)
        {
            return OtherTruthDamageValue;
        }

        return 0;
    }
    public float GetOtherTruthDamageValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherTruthDamageValue;
        }

        if (entityID == OtherID)
        {
            return SelfTruthDamageValue;
        }

        return 0;
    }
    
    public void SetTruthDamageValue(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfTruthDamageValue = value;
        }

        if (entityID == OtherID)
        {
            OtherTruthDamageValue = value;
        }
    }
    #endregion

    #region 打的血量

    private float SelfHpValue { get; set; }
    private float OtherHpValue { get; set; }
    public float GetSelfHpValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfHpValue;
        }

        if (entityID == OtherID)
        {
            return OtherHpValue;
        }

        return 0;
    }
    public float GetOtherHpValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherHpValue;
        }

        if (entityID == OtherID)
        {
            return SelfHpValue;
        }

        return 0;
    }
    public void SetHpValue(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfHpValue = value;
        }

        if (entityID == OtherID)
        {
            OtherHpValue = value;
        }
    }

    #endregion

    #region 打的护盾

    private float SelfShieldValue { get; set; }
    private float OtherShieldValue { get; set; }
    public float GetSelfShieldValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfShieldValue;
        }

        if (entityID == OtherID)
        {
            return OtherShieldValue;
        }

        return 0;
    }
    public float GetOtherShieldValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherShieldValue;
        }

        if (entityID == OtherID)
        {
            return SelfShieldValue;
        }

        return 0;
    }
    public void SetShieldValue(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfShieldValue = value;
        }

        if (entityID == OtherID)
        {
            OtherShieldValue = value;
        }
    }

    #endregion

    #region 打的甲

    private float SelfArmorValue { get; set; }
    private float OtherArmorValue { get; set; }
    public float GetSelfArmorValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return SelfArmorValue;
        }

        if (entityID == OtherID)
        {
            return OtherArmorValue;
        }

        return 0;
    }
    public float GetOtherArmorValue(int entityID)
    {
        if (entityID == SelfID)
        {
            return OtherArmorValue;
        }

        if (entityID == OtherID)
        {
            return SelfArmorValue;
        }

        return 0;
    }
    public void SetArmorValue(int entityID, float value)
    {
        if (entityID == SelfID)
        {
            SelfArmorValue = value;
        }

        if (entityID == OtherID)
        {
            OtherArmorValue = value;
        }
    }


    #endregion
    

    public bool CheckClashIsWin(int entityID)
    {
        if (BattleClashType == BattleClashType.SingleAction)
        {
            return false;
        }

        if (SelfID == entityID && SelfClashState)
        {
            return true;
        }
        
        if (OtherID == entityID && OtherClashState)
        {
            return true;
        }

        return false;
    }
    
    public void Recycle()
    {
        BattleClashType = BattleClashType.None;
        SelfID = 0;
        OtherID = 0;
        SelfSkillID = 0;
        SelfVariantID = 0;
        OtherSkillID = 0;
        OtherVariantID = 0;
        SelfSkillType = SkillType.None;
        OtherSkillType = SkillType.None;
        SelfGangQiCost = 0;
        OtherGangQiCost = 0;
        SelfXuanQiCost = 0;
        OtherXuanQiCost = 0;
        SelfKeyCost.Clear();
        OtherKeyCost.Clear();
        SelfDamageType = DamageType.None;
        OtherDamageType = DamageType.None;
        SelfSource = BattleSource.None;
        OtherSource = BattleSource.None;
        SelfClashState = false;
        OtherClashState = false;
        SelfUseSuccess = false;
        OtherUseSuccess = false;
        SelfFinalDamageRate = 0;
        OtherFinalDamageRate = 0;
        SelfTruthDamageValue = 0;
        OtherTruthDamageValue = 0;
        SelfHpValue = 0;
        OtherHpValue = 0;
        SelfShieldValue = 0;
        OtherShieldValue = 0;
        SelfArmorValue = 0;
        OtherArmorValue = 0;
    }
}
