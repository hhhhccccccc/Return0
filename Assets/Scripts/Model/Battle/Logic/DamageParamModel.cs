using System.Collections.Generic;
using cfg;

public class DamageParamDataModel
{
    public int ID { get; set; }//ID
    public int SkillID { get; set; }//技能ID
    public int VariantID { get; set; }//变式ID
    public SkillType SkillType {get;set;}//技能类型
    public float GangQiCost { get; set; }//刚气消耗
    public float XuanQiCost { get; set; }//玄气消耗
    public List<BattleKey> KeyCost { get; set; } = new();//键消耗
    public DamageType DamageType { get; set; }//伤害类型
    public BattleSource BattleSource { get; set; }//源
    public float DefaultDamageWelly { get; set; }//初始威力
    public float FinalDamageWelly { get; set; }//最终威力
    public bool ClashState { get; set; }//交锋结果
    public bool SkillUseSuccess { get; set; }//技能是否释放成功
    public float AttackTruthDamageValue { get; set; }//折前伤害
    public float AttackHpValue { get; set; }//打的血
    public float AttackShieldValue { get; set; }//打的盾
    public float AttackArmorValue { get; set; }//打的甲
    public bool IsReduceMaxHp { get; set; } //是否扣除体上限
    public bool BeAddCounterBuff { get; set; } //是否被添加了破招buff
    public bool BeTriggerCounterBuff { get; set; } //是否被触发了破招buff 
    public bool ReleaseSkillSuccess { get; set; } //技能是否释放成功 
}

public class DamageParamModel : MomentParamModel, IRecycle
{
    public BattleClashType BattleClashType { get; set; }

    private DamageParamDataModel SelfModel = new();
    private DamageParamDataModel OtherModel = new();
    
    private DamageParamDataModel GetSelfModel(int entityID)
    {
        if (entityID == SelfModel.ID)
        {
            return SelfModel;
        }

        if (entityID == OtherModel.ID)
        {
            return OtherModel;
        }

        return null;
    }
    
    private DamageParamDataModel GetOtherModel(int entityID)
    {
        if (entityID == SelfModel.ID)
        {
            return OtherModel;
        }

        if (entityID == OtherModel.ID)
        {
            return SelfModel;
        }

        return null;
    }
    
    #region 目标
    public void SetSelfID(int selfID)
    {
        SelfModel.ID = selfID;
    }
    public void SetOtherID(int otherID)
    {
        OtherModel.ID = otherID;
    }
    public int GetSelfID(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.ID;
        }

        return 0;
    }
    public int GetOtherID(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.ID;
        }

        return 0;
    }
    #endregion

    #region 技能ID
    public int GetSelfSkillID(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.SkillID;
        }
        
        return 0;
    }
    public int GetOtherSkillID(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.SkillID;
        }
        
        return 0;
    }
    public void SetSkillID(int entityID, int skillID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        { 
            model.SkillID = skillID;
        }
    }
    #endregion

    #region 变式ID
    public int GetSelfVariantID(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.VariantID;
        }
        
        return 0;
    }
    public int GetOtherVariantID(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.VariantID;
        }
        
        return 0;
    }
    public void SetVariantID(int entityID, int variantID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        { 
            model.VariantID = variantID;
        }
    }
    #endregion

    #region 技能类型
    public SkillType GetSelfSkillType(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.SkillType;
        }
        
        return SkillType.None;
    }
    public SkillType GetOtherSkillType(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.SkillType;
        }
        
        return SkillType.None;
    }
    public void SetSkillType(int entityID, SkillType skillType)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        { 
            model.SkillType = skillType;
        }
    }

    #endregion

    #region 资源消耗
    public float GetSelfGangQiCost(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.GangQiCost;
        }

        return 0;
    }

    public float GetOtherGangQiCost(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.GangQiCost;
        }

        return 0;
    }
    public void SetGangQiCost(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.GangQiCost = value;
        }
    }
    
    public float GetSelfXuanQiCost(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.XuanQiCost;
        }

        return 0;
    }

    public float GetOtherXuanQiCost(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.XuanQiCost;
        }

        return 0;
    }
    public void SetXuanQiCost(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.XuanQiCost = value;
        }
    }
    
    public List<BattleKey> GetSelfKeyCost(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.KeyCost;
        }

        return null;
    }

    public List<BattleKey> GetOtherKeyCost(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.KeyCost;
        }

        return null;
    }
    public void SetKeyCost(int entityID, List<BattleKey> keyList)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.KeyCost.AddRange(keyList);
        }
    }
    
    #endregion

    #region 伤害类型

    public DamageType GetSelfDamageType(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.DamageType;
        }

        return DamageType.None;
    }
    public DamageType GetOtherDamageType(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.DamageType;
        }

        return DamageType.None;
    }
    public void SetDamageType(int entityID, DamageType damageType)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.DamageType = damageType;
        }
    }
    #endregion

    #region 源
    public BattleSource GetSelfBattleSource(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.BattleSource;
        }

        return BattleSource.None;
    }
    public BattleSource GetOtherBattleSource(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.BattleSource;
        }

        return BattleSource.None;
    }
    public void SetBattleSource(int entityID, BattleSource sourceType)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.BattleSource = sourceType;
        }
    }
    #endregion

    #region 初始伤害威力
    public float GetSelfDefaultDamageWelly(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.DefaultDamageWelly;
        }

        return 0;
    }
    public float GetOtherDefaultDamageWelly(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.DefaultDamageWelly;
        }

        return 0;
    }
    public void SetDefaultDamageWelly(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.DefaultDamageWelly = value;
        }
    }
    #endregion
    
    #region 最终伤害威力
    public float GetSelfFinalDamageWelly(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.FinalDamageWelly;
        }

        return 0;
    }
    public float GetOtherFinalDamageWelly(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.FinalDamageWelly;
        }

        return 0;
    }
    public void SetFinalDamageWelly(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.FinalDamageWelly = value;
        }
    }
    #endregion

    #region 交锋结果
    public bool GetSelfClashState(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.ClashState;
        }

        return false;
    }
    public bool GetOtherClashState(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.ClashState;
        }

        return false;
    }
    public void SetClashState(int entityID, bool state)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.ClashState = state;
        }
    }
    #endregion

    #region 技能是否释放成功
    public bool GetSelfSkillUseSuccess(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.SkillUseSuccess;
        }

        return false;
    }
    public bool GetOtherSkillUseSuccess(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.SkillUseSuccess;
        }

        return false;
    }
    public void SetUseSuccess(int entityID, bool state)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.SkillUseSuccess = state;
        }
    }

    #endregion

    #region 真实伤害
    public float GetSelfAttackTruthDamageValue(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.AttackTruthDamageValue;
        }

        return 0;
    }
    public float GetOtherAttackTruthDamageValue(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.AttackTruthDamageValue;
        }

        return 0;
    }
    
    public void SetAttackTruthDamageValue(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.AttackTruthDamageValue = value;
        }
    }
    #endregion

    #region 打的血量
    public float GetSelfAttackHpValue(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.AttackHpValue;
        }

        return 0;
    }
    public float GetOtherAttackHpValue(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.AttackHpValue;
        }

        return 0;
    }
    public void SetAttackHpValue(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.AttackHpValue = value;
        }
    }

    #endregion

    #region 打的护盾
    public float GetSelfAttackShieldValue(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.AttackShieldValue;
        }

        return 0;
    }
    public float GetOtherAttackShieldValue(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.AttackShieldValue;
        }

        return 0;
    }
    public void SetAttackShieldValue(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.AttackShieldValue = value;
        }
    }

    #endregion

    #region 打的甲
    public float GetSelfAttackArmorValue(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.AttackArmorValue;
        }

        return 0;
    }
    public float GetOtherAttackArmorValue(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.AttackArmorValue;
        }

        return 0;
    }
    public void SetAttackArmorValue(int entityID, float value)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.AttackArmorValue = value;
        }
    }
    
    #endregion

    #region 是否扣除体上限
    public bool GetSelfDamageReduceMaxHp(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.IsReduceMaxHp;
        }

        return false;
    }
    public bool GetOtherDamageReduceMaxHp(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.IsReduceMaxHp;
        }

        return false;
    }
    public void SetDamageReduceMaxHp(int entityID, bool state)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.IsReduceMaxHp = state;
        }
    }
    #endregion
    
    #region 是否被添加了破招buff
    public bool GetSelfBeAddCounterBuff(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.BeAddCounterBuff;
        }

        return false;
    }
    public bool GetOtherBeAddCounterBuff(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.BeAddCounterBuff;
        }

        return false;
    }
    public void SetBeAddCounterBuff(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.BeAddCounterBuff = true;
        }
    }
    #endregion
    
    #region 是否触发了破招buff
    public bool GetSelfBeTriggerCounterBuff(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.BeTriggerCounterBuff;
        }

        return false;
    }
    
    public bool GetOtherBeTriggerCounterBuff(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.BeTriggerCounterBuff;
        }

        return false;
    }
    
    public void SetBeTriggerCounterBuff(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.BeAddCounterBuff = true;
        }
    }
    #endregion
    
    #region 是否释放成功
    public bool GetSelfReleaseSkillSuccess(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.ReleaseSkillSuccess;
        }

        return false;
    }
    
    public bool GetOtherReleaseSkillSuccess(int entityID)
    {
        var model = GetOtherModel(entityID);
        if (model != null)
        {
            return model.ReleaseSkillSuccess;
        }

        return false;
    }
    
    public void SetReleaseSkillSuccess(int entityID)
    {
        var model = GetSelfModel(entityID);
        if (model != null)
        {
            model.ReleaseSkillSuccess = true;
        };
    }
    #endregion
    
    public bool CheckClashIsWin(int entityID)
    {
        if (BattleClashType == BattleClashType.SingleAction)
        {
            return false;
        }

        var model = GetSelfModel(entityID);
        if (model != null)
        {
            return model.ClashState;
        }

        return false;
    }

    public float GetDirectDamageValue(int entityID)
    {
        if (GetSelfSkillType(entityID) == SkillType.PowerKilling || GetSelfSkillType(entityID) == SkillType.ArtKilling)
        {
            if (GetSelfDamageReduceMaxHp(entityID))
            {
                return 0;
            }

            return GetSelfAttackHpValue(entityID);
        }

        return 0;
    }
    
    public void Recycle()
    {
        BattleClashType = BattleClashType.None;
        SelfModel.ID = 0;
        OtherModel.ID = 0;
        SelfModel.SkillID = 0;
        OtherModel.SkillID = 0;
        SelfModel.VariantID = 0;
        OtherModel.VariantID = 0;
        SelfModel.SkillType = SkillType.None;
        OtherModel.SkillType = SkillType.None;
        SelfModel.GangQiCost = 0;
        OtherModel.GangQiCost = 0;
        SelfModel.XuanQiCost = 0;
        OtherModel.XuanQiCost = 0;
        SelfModel.KeyCost.Clear();
        OtherModel.KeyCost.Clear();
        SelfModel.DamageType = DamageType.None;
        OtherModel.DamageType = DamageType.None;
        SelfModel.BattleSource = BattleSource.None;
        OtherModel.BattleSource = BattleSource.None;
        SelfModel.ClashState = false;
        OtherModel.ClashState = false;
        SelfModel.SkillUseSuccess = false;
        OtherModel.SkillUseSuccess = false;
        SelfModel.DefaultDamageWelly = 0;
        OtherModel.DefaultDamageWelly = 0;
        SelfModel.FinalDamageWelly = 0;
        OtherModel.FinalDamageWelly = 0;
        SelfModel.AttackTruthDamageValue = 0;
        OtherModel.AttackTruthDamageValue = 0;
        SelfModel.AttackHpValue = 0;
        OtherModel.AttackHpValue = 0;
        SelfModel.AttackShieldValue = 0;
        OtherModel.AttackShieldValue = 0;
        SelfModel.AttackArmorValue = 0;
        OtherModel.AttackArmorValue = 0;
        SelfModel.IsReduceMaxHp = false;
        OtherModel.IsReduceMaxHp = false;
        SelfModel.BeAddCounterBuff = false;
        OtherModel.BeAddCounterBuff = false;
    }
}
