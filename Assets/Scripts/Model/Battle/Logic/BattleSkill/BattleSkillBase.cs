using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleSkillBase : BattleSkillMoment, IModel, IRecycle
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private BattleUtil BattleUtil;

    public int SkillID { get; private set; }

    public BattleUnit Subject { get; private set; }

    public BattleUnit Target { get; private set; }

    public BattleSkillConfig Config { get; private set; }

    /// <summary>
    /// 技能刚炁消耗
    /// </summary>
    private float GangQiCost { get; set; }
    public void SetGangQiCost(float gangQiCost) => GangQiCost = gangQiCost;
    public float GetGangQiCost() => GangQiCost;

    /// <summary>
    /// 技能玄炁消耗
    /// </summary>
    private float XuanQiCost { get; set; }
    public void SetXuanQiCost(float xuanQiCost) => XuanQiCost = xuanQiCost;
    public float GetXuanQiCost() => XuanQiCost;

    /// <summary>
    /// 技能的键消耗
    /// </summary>
    private List<int> KeyCostList { get; set; }
    public List<int> GetKeyCostList => KeyCostList;
    
    /// <summary>
    /// 技能威力
    /// </summary>
    private float SkillDamageRate { get; set; }
    public float GetSkillDamageRate => SkillDamageRate;
    public void SetSkillDamageRate(float damageRate) => SkillDamageRate = damageRate;

    /// <summary>
    /// 在行动期间是否被攻击过
    /// </summary>
    private bool BeDamageInSkillAction{ get; set; }
    public void SetBeDamageInSkillAction() => BeDamageInSkillAction = true;
    public bool GetBeDamageInSkillAction() => BeDamageInSkillAction;

    /// <summary>
    /// 技能类型
    /// </summary>
    private SkillType SkillType{ get; set; }
    public void SetSkillType(SkillType skillType) => SkillType = skillType;
    public SkillType GetSKillType => SkillType;

    /// <summary>
    /// 伤害类型
    /// </summary>
    private DamageType DamageType { get; set; }
    public void SetDamageType(DamageType damageType) => DamageType = damageType;
    public DamageType GetDamageType => DamageType;
    public List<int> GetRemoveMomentList => Config.SkillRemoveMoment;
    
    /// <summary>
    /// 判断技能期间经过了哪些阶段
    /// </summary>
    private HashSet<int> PassMomentList = new();
    private void AddPassMoment(BattleMomentType momentType)
    {
        PassMomentList.Add((int)momentType);
    }
    public bool CheckTriggerMoment(BattleMomentType momentType) => PassMomentList.Contains((int)momentType);
    public void Init(int skillID, BattleUnit subject, BattleUnit target)
    {
        SkillID = skillID;
        Config = ConfigManager.GetBattleSkillConfig(skillID);
        Subject = subject;
        SetTarget(target);
        BeDamageInSkillAction = false;
        PassMomentList.Clear();
        var preUseMgr = subject.PreUseSkillDataManager;
        SetGangQiCost(preUseMgr.GetSkillPreUseGangQiCost(skillID));
        SetXuanQiCost(preUseMgr.GetSkillPreUseXuanQiCost(skillID));
        KeyCostList = preUseMgr.GetSkillPreUseKeyCost(skillID);
        SetSkillDamageRate(preUseMgr.GetSkillPreUseDamage(skillID));
        SetSkillType(preUseMgr.GetSkillPreUseSkillType(skillID));
        SetDamageType(preUseMgr.GetSkillPreUseDamageType(skillID));
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
        AddPassMoment(BattleMomentType.AfterAction);
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