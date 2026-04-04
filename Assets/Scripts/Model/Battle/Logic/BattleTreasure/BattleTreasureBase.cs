using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleTreasureBase : BattleMoment
{
    public int TreasureID { get; set; }
    public TreasureConfig Config { get; set; }
    protected override float GetConfigParamFloat(int index) => Config.ParamList[index];
    public override int GetConfigParamInt(int index) => Config.ParamList[index].ToRound();
    protected override int GetSymbol => 400000 + Config.Id;
    public virtual void Init(int treasureID, BattleUnit subject)
    {
        TreasureID = treasureID;
        Subject = subject;
        Config = ConfigManager.GetTreasureConfig(treasureID);
    }
        
        
    private bool CanTrigger()
    {
        var aliveUnitList = BattleManager.GetAllAliveUnit();
        if (aliveUnitList.Any(unit => unit.BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10095)))
        {
            return false;
        }

        return true;
    }
    
    #region 战斗改变属性机制
    
    public override float GetWellyRateEx(int skillGuid)
    {
        if (!CanTrigger())
        {
            return 0;
        }
        return OnGetSkillWellyRate(skillGuid);
    }
    protected virtual float OnGetSkillWellyRate(int skillGuid) => 0;

    public override float GetWellyIncrease(int skillGuid)
    {
        if (!CanTrigger())
        {
            return 0;
        }
        return 0;
    }
    
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!CanTrigger())
        {
            return 0;
        } 

        return OnGetProperty(propertyType, model);
    }
    protected virtual float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null) => 0;
    


    public override float AttackDamageAddPct(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return OnGetSkillDamageRate(paramModel);
    }
    protected virtual float OnGetSkillDamageRate(MomentParamModel paramModel) => 0;
    
 
    
    public override  void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnAddDamageValueInt(dict, paramModel);
    }
    protected virtual void OnAddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel) {}
    
 

    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        if (!CanTrigger())
        {
            return true;
        }
        return OnCheckCanAddBuff(buffID, ref addCount, spellCasterID, momentType);
    }

    protected virtual bool OnCheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType = BattleMomentType.None) => true;

    public override bool IgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return false;
        }
        return OnCanIgnoreSkillDirectDamage(paramModel);
    }
    protected virtual bool OnCanIgnoreSkillDirectDamage(MomentParamModel paramModel) => false;


    public override float BeDamageReducePct(int attackID, DamageType damageType)
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return OnBeDamageReducePct(attackID, damageType);
    }
    protected virtual float OnBeDamageReducePct(int attackID, DamageType damageType) => 0;

    public override void BeforeAttack(MomentParamModel model)
    {
        if (!CanTrigger())
        {
            return;
        }
        
        OnBeforeAttack(model);
    }
    protected virtual void OnBeforeAttack(MomentParamModel model) {}

    public override void BeDamage(DamageType damageType)
    {
        if (!CanTrigger())
        {
            return;
        }
        
        OnBeDamage(damageType);
    }
    protected virtual void OnBeDamage(DamageType damageType) {}
    
    public override void TryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (!CanTrigger())
        {
            return;
        }

        OnTryStoreBattleKey(keyType, ref count);
    }
    protected virtual void OnTryStoreBattleKey(BattleKeyType keyType, ref int count) {}
    
    #endregion

    protected override void OnRecycle()
    {
        OnTreasureRecycle();
    }
    protected virtual void OnTreasureRecycle() {}

    protected BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType, params float[] values)
    {
        var viewModel = base.AllocViewModel(entityID, viewType);
        if (values.Length > 0)
        {
            foreach (var value in values)
            {
                viewModel.FloatParam.Add(value);
            }
        }

        return viewModel;
    }
    
    public override void EnqueueViewModel(BattleMomentViewModel viewModel)
    {
        BattleRecordManager.AddBattleMomentViewModel(viewModel);
    }

    public override BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType)
    {
        var viewModel = PM.GetClass<BattleMomentViewModel>();
        viewModel.BattleSource = BattleSource.Treasure;
        viewModel.EntityID = entityID;
        viewModel.ConfigID = TreasureID;
        return viewModel;
    }
    
    protected void EnqueueViewModel(int entityID, MomentViewType viewType, params float[] values)
    {
        EnqueueViewModel(AllocViewModel(entityID, viewType, values)); 
    }

    #region 扳机

    public override void BattleStart()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnBattleStart();
    }
    protected virtual void OnBattleStart(){}

    public override void RoundStart()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnRoundStart();
    }
    protected virtual void OnRoundStart() {}

    public override void CalculateActionWheel()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnCalculateActionWheel();
    }
    protected virtual void OnCalculateActionWheel(){}

    public override void BeforeDoDesitionAction()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnBeforeDoDesitionAction();
    }
    protected virtual void OnBeforeDoDesitionAction(){}
    
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnDoDesitionAction(isPreDesition);
    }
    protected virtual void OnDoDesitionAction(bool isPreDesition){}

    public override void EveryActionWheelStart()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnEveryActionWheelStart();
    }
    protected virtual void OnEveryActionWheelStart(){}

    public override void SelfActionWheelStart()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnSelfActionWheelStart();
    }
    protected virtual void OnSelfActionWheelStart(){}

    public override void BeforeAction()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnBeforeAction();
    }
    protected virtual void OnBeforeAction(){}

    
    public override void BeforeUnderAction()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnBeforeUnderAction();
    }
    protected virtual void OnBeforeUnderAction(){}

    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnBeforeClash(paramModel);
    }
    protected virtual void OnBeforeClash(MomentParamModel paramModel){}
    
    public override void AfterClash(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnAfterClash(paramModel);
    }
    protected virtual void OnAfterClash(MomentParamModel paramModel){}
    
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnReleaseSkillAction(paramModel);
    }
    protected virtual void OnReleaseSkillAction(MomentParamModel paramModel){}

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnAfterUnderAction(paramModel);
    }
    protected virtual void OnAfterUnderAction(MomentParamModel paramModel){}
    
    public override void AfterAction(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        OnAfterAction(paramModel);
    }
    protected virtual void OnAfterAction(MomentParamModel paramModel) {}

    public override void ActionWheelEnd()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnActionWheelEnd();
    }
    protected virtual void OnActionWheelEnd(){}

    public override void RoundEnd()
    {
        if (!CanTrigger())
        {
            return;
        }
        OnRoundEnd();
    }
    protected virtual void OnRoundEnd(){}

    public override void BattleEnd()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBattleEnd();
    }
    protected virtual void OnBattleEnd() {}

    #endregion
}
