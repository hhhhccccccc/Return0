using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleSkillBase : BattleSkillMoment, IModel, IRecycle
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private BattleUtil BattleUtil;

    public int SkillID;

    public BattleUnit Subject;

    public BattleUnit Target;

    public BattleSkillConfig Config;

    /// <summary>
    /// 技能刚炁消耗
    /// </summary>
    private float GangQiCost;

    public void SetGangQiCost(float gangQiCost) => GangQiCost = gangQiCost;
    public float GetGangQiCost() => GangQiCost;

    /// <summary>
    /// 技能玄炁消耗
    /// </summary>
    private float XuanQiCost;

    public void SetXuanQiCost(float xuanQiCost) => XuanQiCost = xuanQiCost;
    public float GetXuanQiCost() => XuanQiCost;

    /// <summary>
    /// 技能的键消耗
    /// </summary>
    private List<int> KeyCostList;

    public List<int> GetKeyCostList => KeyCostList;

    private float SkillDamageRate;
    public float GetSkillDamageRate => SkillDamageRate;
    public void SetSkillDamageRate(float damageRate) => SkillDamageRate = damageRate;

    /// <summary>
    /// 在行动期间是否被攻击过
    /// </summary>
    private bool BeDamageInSkillAction;

    public void SetBeDamageInSkillAction() => BeDamageInSkillAction = true;
    public bool GetBeDamageInSkillAction() => BeDamageInSkillAction;

    /// <summary>
    /// 技能类型
    /// </summary>
    private SkillType SkillType;

    public void SetSkillType(SkillType skillType) => SkillType = skillType;
    public SkillType GetSKillType => SkillType;

    /// <summary>
    /// 伤害类型
    /// </summary>
    private DamageType DamageType;

    public void SetDamageType(DamageType damageType) => DamageType = damageType;
    public DamageType GetDamageType => DamageType;
    public List<int> GetRemoveMomentList => Config.SkillRemoveMoment;
    
    /// <summary>
    /// 判断技能期间经过了哪些阶段
    /// </summary>
    private HashSet<int> TriggerMomentList = new();
    private void AddTriggerMoment(BattleMomentType momentType)
    {
        TriggerMomentList.Add((int)momentType);
    }
    public bool CheckTriggerMoment(BattleMomentType momentType) => TriggerMomentList.Contains((int)momentType);
    public void Init(int skillID, BattleUnit subject, BattleUnit target)
    {
        SkillID = skillID;
        Config = ConfigManager.GetBattleSkillConfig(skillID);
        Subject = subject;
        SetTarget(target);
        BeDamageInSkillAction = false;
        TriggerMomentList.Clear();
        var useData = subject.GetSkillPreUseData(skillID);
        SetGangQiCost(useData.GetGangQiCost());
        SetXuanQiCost(useData.GetXuanQiCost());
        KeyCostList = useData.GetKeyCost();
        SetSkillDamageRate(Config.Damage);
        SetSkillType((SkillType)Config.SkillType);
        SetDamageType((DamageType)Config.DamageType);
        InitMoment(this);
    }

    /// <summary>
    /// 技能结束的时候调用技能结束扳机
    /// </summary>
    public void SkillEnd()
    {
        var subjectID = Subject.EntityID;
        foreach (var momentID in Config.SkillEndMoment)
        {
            EnqueueViewModel(BattleMomentType.SkillEnd, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public bool SkillIsKillingStyle()
    {
        return BattleUtil.SkillIsKillingStyle(GetSKillType);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        AddTriggerMoment(BattleMomentType.AfterAction);
    }

    public int GetSkillID()
    {
        return SkillID;
    }

    public BattlePropertyType GetFirstKeyType()
    {
        return (BattlePropertyType)GetKeyCostList[0];
    }

    public string GetSkillAniName() => Config.AniName;

    public void SetTarget(BattleUnit newTarget)
    {
        Target = newTarget;
    }

    public void Recycle()
    {
        SkillID = 0;
    }
}