using System.Collections.Generic;
using cfg;
using Zenject;

public abstract class BattleRecordModel : IModel, IRecycle
{
    [Inject] protected IPoolManager PoolManager;
    public virtual BattleClashType BattleClashType { get; protected set; }
    public int SubjectID{ get; set; }
    public int TargetID { get; set; }

    //行动前判定 能否拉起单方面行动
    public bool CheckSubjectBeCounter { get; set; }
    public bool CheckSubjectCostPullFight { get; set; }
    protected Queue<BattleMomentViewModel> Subject_BeforeAction { get; set; } = new();
    protected Queue<BattleMomentViewModel> Target_BeforeUnderAction { get; set; } = new();
    public bool CheckSubjectCostGenerateAction { get; set; }

    protected int Subject_UseSkillID { get; set; }
    protected SkillType Subject_SkillType { get; set; }
    protected float Subject_SkillDamageRateDefault { get; set; }
    protected float Subject_SkillDamageRateFinal { get; set; }
    protected BattleSource Subject_BattleSource { get; set; }
    protected DamageType Subject_DamageType { get; set; }
    protected float Subject_TruthDamage { get; set; }
    protected float Subject_AttackHpValue { get; set; }
    protected float Subject_AttackShieldValue { get; set; }
    protected float Subject_AttackArmorValue { get; set; }
    protected float Subject_GangQiCost { get; set; }
    protected float Subject_XuanQiCost { get; set; }
    protected List<int> Subject_KeyCost { get; set; }
 
    protected int Target_UseSkillID { get; set; }
    protected SkillType Target_SkillType { get; set; }
    protected float Target_SkillDamageRateDefault { get; set; }
    protected float Target_SkillDamageRateFinal { get; set; }
    protected BattleSource Target_BattleSource { get; set; }
    protected DamageType Target_DamageType { get; set; }
    protected float Target_TruthDamage { get; set; }
    protected float Target_AttackHpValue { get; set; }
    protected float Target_AttackShieldValue { get; set; }
    protected float Target_AttackArmorValue { get; set; }
    protected float Target_GangQiCost { get; set; }
    protected float Target_XuanQiCost { get; set; }
    protected List<int> Target_KeyCost { get; set; }
    
    public bool Subject_AddCounterBuff { get; set; }
    public bool Target_AddCounterBuff { get; set; }
    
    public bool Subject_TriggerCounterBuff { get; set; }
    public bool Target_TriggerCounterBuff { get; set; }
    
    public bool CheckSubjectCostBeforeReleaseSkill { get; set; }
    public bool CheckTargetCostBeforeReleaseSkill { get; set; }
    
    protected Queue<BattleMomentViewModel> Subject_ReleaseSkillAction { get; set; } = new();
    protected Queue<BattleMomentViewModel> Target_ReleaseSkillAction { get; set; } = new();

    protected Queue<BattleMomentViewModel> Target_AfterUnderAction { get; set; }= new();
    protected Queue<BattleMomentViewModel> Subject_AfterAction { get; set; } = new();

    public void SetReleaseSkillSuccess(int entityID)
    {
        if (SubjectID == entityID)
        {
            CheckSubjectCostBeforeReleaseSkill = true;
        }

        if (TargetID == entityID)
        {
            CheckTargetCostBeforeReleaseSkill = true;
        }
    }
    
    public bool GetReleaseSkillSuccess(int entityID)
    {
        if (SubjectID == entityID)
        {
            return CheckSubjectCostBeforeReleaseSkill;
        }

        if (TargetID == entityID)
        {
            return CheckTargetCostBeforeReleaseSkill;
        }

        return false;
    }

    public abstract void AddBattleMomentViewModel(BattleMomentViewModel viewModel);
    
    public abstract Queue<BattleMomentViewModel> GetQueue(BattleMomentType momentType, int entityID);
    
    public virtual void Recycle()
    {
        BattleClashType = BattleClashType.None;
        CheckSubjectBeCounter = false;
        
        CheckSubjectCostPullFight = false;
        CheckSubjectCostGenerateAction = false;

        Subject_UseSkillID = 0;
        Subject_SkillDamageRateDefault = 0;
        Subject_SkillDamageRateFinal = 0;
        Subject_BattleSource = BattleSource.None;
        Subject_DamageType = DamageType.None;
        Subject_TruthDamage = 0;
        Subject_AttackHpValue = 0;
        Subject_AttackShieldValue = 0;
        Subject_AttackArmorValue = 0;
        Subject_GangQiCost = 0;
        Subject_XuanQiCost = 0;
        Subject_KeyCost = null;

        Target_UseSkillID = 0;
        Target_SkillDamageRateDefault = 0;
        Target_SkillDamageRateFinal = 0;
        Target_BattleSource = BattleSource.None;
        Target_DamageType = DamageType.None;
        Target_TruthDamage = 0;
        Target_AttackHpValue = 0;
        Target_AttackArmorValue = 0;
        Target_GangQiCost = 0;
        Target_XuanQiCost = 0;
        Target_KeyCost = null;
        
        Subject_AddCounterBuff = false;
        Target_AddCounterBuff = false;
        Subject_TriggerCounterBuff = false;
        Target_TriggerCounterBuff = false;

        CheckSubjectCostBeforeReleaseSkill = false;
        CheckTargetCostBeforeReleaseSkill = false;

        foreach (var viewModel in Subject_BeforeAction)
        {
            PoolManager.RecycleClass(viewModel);
        }

        Subject_BeforeAction.Clear();

        foreach (var viewModel in Target_BeforeUnderAction)
        {
            PoolManager.RecycleClass(viewModel);
        }
        
        foreach (var viewModel in Subject_ReleaseSkillAction)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Subject_ReleaseSkillAction.Clear();
        
        foreach (var viewModel in Target_ReleaseSkillAction)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Target_ReleaseSkillAction.Clear();
        
        foreach (var viewModel in Subject_AfterAction)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Subject_AfterAction.Clear();
        
        foreach (var viewModel in Target_AfterUnderAction)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Target_AfterUnderAction.Clear();

        Target_BeforeUnderAction.Clear();
    }

    public void SetAddCounterBuff(int entityID)
    {
        if (entityID == SubjectID)
        {
            Subject_AddCounterBuff = true;
        }
        if (entityID == TargetID)
        {
            Target_AddCounterBuff = true;
        }
    }

    public void SetTriggerCounterBuff(int entityID)
    {
        if (entityID == SubjectID)
        {
            Subject_TriggerCounterBuff = true;
        }
        if (entityID == TargetID)
        {
            Target_TriggerCounterBuff = true;
        }
    }
    
    public void SetSkillID(int entityID, int skillID)
    {
        if (entityID == SubjectID)
        {
            Subject_UseSkillID = skillID;
        }
        if (entityID == TargetID)
        {
            Target_UseSkillID = skillID;
        }
    }

    public int GetSkillID(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_UseSkillID;
        }
        if (entityID == TargetID)
        {
            return Target_UseSkillID;
        }

        return 0;
    }
    
    public void SetSkillType(int entityID, SkillType skillType)
    {
        if (entityID == SubjectID)
        {
            Subject_SkillType = skillType;
        }
        if (entityID == TargetID)
        {
            Target_SkillType = skillType;
        }
    }

    public SkillType GetSkillType(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_SkillType;
        }
        if (entityID == TargetID)
        {
            return Target_SkillType;
        }

        return 0;
    }

    public void SetSkillDamageRateDefault(int entityID, float damageRate)
    {
        if (entityID == SubjectID)
        {
            Subject_SkillDamageRateDefault = damageRate;
        }
        if (entityID == TargetID)
        {
            Target_SkillDamageRateDefault = damageRate;
        }
    }
    
    public float GetSkillDamageRateDefault(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_SkillDamageRateDefault;
        }
        if (entityID == TargetID)
        {
            return Target_SkillDamageRateDefault;
        }

        return 0;
    }

    public void SetSkillDamageRateFinal(int entityID, float damageRate)
    {
        if (entityID == SubjectID)
        {
            Subject_SkillDamageRateFinal = damageRate;
        }
        if (entityID == TargetID)
        {
            Target_SkillDamageRateFinal = damageRate;
        }
    }

    public float GetSkillDamageRateFinal(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_SkillDamageRateFinal;
        }
        if (entityID == TargetID)
        {
            return Target_SkillDamageRateFinal;
        }

        return 0;
    }
    
    public void SetBattleSource(int entityID, BattleSource battleSource)
    {
        if (entityID == SubjectID)
        {
            Subject_BattleSource = battleSource;
        }
        if (entityID == TargetID)
        {
            Target_BattleSource = battleSource;
        }
    }
    
    public BattleSource SetBattleSource(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_BattleSource;
        }
        if (entityID == TargetID)
        {
            return Target_BattleSource;
        }

        return BattleSource.None;
    }

    public void SetDamageType(int entityID, DamageType damageType)
    {
        if (entityID == SubjectID)
        {
            Subject_DamageType = damageType;
        }
        if (entityID == TargetID)
        {
            Target_DamageType = damageType;
        }
    }
    
    public DamageType GetDamageType(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_DamageType;
        }
        if (entityID == TargetID)
        {
            return Target_DamageType;
        }

        return DamageType.None;
    }

    public void SetTruthDamage(int entityID, float damageValue)
    {
        if (entityID == SubjectID)
        {
            Subject_TruthDamage = damageValue;
        }
        if (entityID == TargetID)
        {
            Target_TruthDamage = damageValue;
        }
    }
    
    public float GetTruthDamage(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_TruthDamage;
        }
        if (entityID == TargetID)
        {
            return Target_TruthDamage;
        }

        return 0;
    }
    
    public void SetAttackHpValue(int entityID, float hpValue)
    {
        if (entityID == SubjectID)
        {
            Subject_AttackHpValue = hpValue;
        }
        if (entityID == TargetID)
        {
            Target_AttackHpValue = hpValue;
        }
    }
    
    public float GetAttackHpValue(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_AttackHpValue;
        }
        if (entityID == TargetID)
        {
            return Target_AttackHpValue;
        }

        return 0;
    }
    
    public void SetAttackShieldValue(int entityID, float shieldValue)
    {
        if (entityID == SubjectID)
        {
            Subject_AttackShieldValue = shieldValue;
        }
        if (entityID == TargetID)
        {
            Target_AttackShieldValue = shieldValue;
        }
    }
    
    public float GetAttackShieldValue(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_AttackShieldValue;
        }
        if (entityID == TargetID)
        {
            return Target_AttackShieldValue;
        }

        return 0;
    }
    
    public void SetAttackArmorValue(int entityID, float armorValue)
    {
        if (entityID == SubjectID)
        {
            Subject_AttackArmorValue = armorValue;
        }
        if (entityID == TargetID)
        {
            Target_AttackArmorValue = armorValue;
        }
    }
    
    public float GetAttackArmorValue(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_AttackArmorValue;
        }
        if (entityID == TargetID)
        {
            return Target_AttackArmorValue;
        }

        return 0;
    }

    public void SetGangQiCost(int entityID, float value)
    {
        if (entityID == SubjectID)
        {
            Subject_GangQiCost = value;
        }
        if (entityID == TargetID)
        {
            Target_GangQiCost = value;
        }
    }
    
    public float GetGangQiCost(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_GangQiCost;
        }
        if (entityID == TargetID)
        {
            return Target_GangQiCost;
        }

        return 0;
    }
    
    public void SetXuanQiCost(int entityID, float value)
    {
        if (entityID == SubjectID)
        {
            Subject_XuanQiCost = value;
        }
        if (entityID == TargetID)
        {
            Target_XuanQiCost = value;
        }
    }
    
    public float GetXuanQiCost(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_XuanQiCost;
        }
        if (entityID == TargetID)
        {
            return Target_XuanQiCost;
        }

        return 0;
    }
    
    public void SetKeyCost(int entityID, List<int> keyCost)
    {
        if (entityID == SubjectID)
        {
            Subject_KeyCost = keyCost;
        }
        if (entityID == TargetID)
        {
            Target_KeyCost = keyCost;
        }
    }
    
    public List<int> GetKeyCost(int entityID)
    {
        if (entityID == SubjectID)
        {
            return Subject_KeyCost;
        }
        
        if (entityID == TargetID)
        {
            return Target_KeyCost;
        }

        return new List<int>();
    }
}
