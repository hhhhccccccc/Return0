using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel, IGetBattlePropertyChanged, IRecycle
{
    #region 事件
    
    private readonly List<IDisposable> _registerDisposables = new();
    //MessageManager
    protected IDisposable Register<T>(Action<T> callback) where T : MessageModel
    {
        IDisposable disposable = this.MessageManager.Register<T>(callback);
        this._registerDisposables.Add(disposable);
        return disposable;
    }
    protected void DispatchMsg<T>(T msg) where T : MessageModel => MessageManager.DispatchMsg(msg);

    #endregion
    
    [Inject] protected IPoolManager PM { get; set; }
    [Inject] protected IMessageManager MessageManager { get; set; }
    [Inject] protected BattleUtil BattleUtil { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] protected ConfigHelper ConfigHelper { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    public int TreasureID { get; set; }
    public BattleUnit Subject { get; set; }
    public TreasureConfig Config { get; set; }
    protected float GetParamFloat(int index) => Config.ParamList[index];
    public int GetParamInt(int index) => Config.ParamList[index].ToRound();
    protected int GetSymbol => 200000 + Config.Id;
    public virtual void Init(int treasureID, BattleUnit subject)
    {
        TreasureID = treasureID;
        Subject = subject;
        Config = ConfigManager.GetTreasureConfig(treasureID);
        InitMoment(this);
    }
        
    #region 战斗改变属性机制
    
    public float GetSkillWelly(int skillGuid)
    {
        if (!CanEffect())
        {
            return 0;
        }
        
        return OnGetSkillWellyRate(skillGuid);
    }

    protected virtual float OnGetSkillWellyRate(int skillGuid) => 0;

    public float GetSkillWellyEffect(int skillGuid)
    {
        if (!CanEffect())
        {
            return 0;
        }
        
        return 0;
    }

    public void TrySetBaseWelly(int skillGuid, ref float value)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void TrySetAddWelly(int skillGuid, ref float value)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public int GetKeyMaxEx()
    {
        if (!CanEffect())
        {
            return 0;
        }

        return 0;
    }
    public void HpChanged()
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void SkillEnd(BattleSkillBase skill)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!CanEffect())
        {
            return 0;
        } 

        return OnGetProperty(propertyType, model);
    }

    public virtual float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null) => 0;
    
    public void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public int GetChangeActionWheel()
    {
        if (!CanEffect())
        {
            return 0;
        }

        return 0;
    }

    public float GetSkillDamageRate(MomentParamModel paramModel)
    {
        if (!CanEffect())
        {
            return 0;
        }

        return OnGetSkillDamageRate(paramModel);
    }
    protected virtual float OnGetSkillDamageRate(MomentParamModel paramModel) => 0;
    
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public virtual void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public float GetReplaceSkillGangQiCost()
    {
        if (!CanEffect())
        {
            return 0;
        }

        return 0;
    }
    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public float GetReplaceSkillXuanQiCost()
    {
        if (!CanEffect())
        {
            return 0;
        }

        return 0;
    }
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public virtual void OnKillUnit(int beKillID)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        if (!CanEffect())
        {
            return (gangQiCost, xuanQiCost);
        }

        return (gangQiCost, xuanQiCost);
    }

    public bool CheckReCalculateDamage(MomentParamModel model)
    {
        return false;
    }

    public void BeforeReduceHp(float reduceHp)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public virtual void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public virtual void EndAction()
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void RemoveBeforeNextAction()
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (!CanEffect())
        {
            return;
        }
        
        OnAddDamageValueInt(dict, paramModel);
    }

    protected virtual void OnAddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        
    }
    
    public void ReduceDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void AfterUnitInit()
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void BeCounter()
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        if (!CanEffect())
        {
            return;
        }
    }

    public bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        if (!CanEffect())
        {
            return true;
        }

        return OnCheckCanAddBuff(buffID, ref addCount, spellCasterID, momentType);
    }

    protected virtual bool OnCheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType = BattleMomentType.None) => true;

    public bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanEffect())
        {
            return false;
        }
        
        return OnCanIgnoreSkillDirectDamage(paramModel);
    }
    protected virtual bool OnCanIgnoreSkillDirectDamage(MomentParamModel paramModel) => false;

    public bool CanBeCounter(MomentParamModel paramModel)
    {
        if (!CanEffect())
        {
            return true;
        }
        return true;
    }

    public float GetDamageReducePct(int attackID, DamageType damageType)
    {
        if (!CanEffect())
        {
            return 0;
        }

        return OnGetDamageReducePct(attackID, damageType);
    }
    protected virtual float OnGetDamageReducePct(int attackID, DamageType damageType) => 0;

    public void BeforeAttack(MomentParamModel model)
    {
        if (!CanEffect())
        {
            return;
        }
        
        OnBeforeAttack(model);
    }
    protected virtual void OnBeforeAttack(MomentParamModel model) {}

    public void BeDamage(MomentParamModel paramModel)
    {
        if (!CanEffect())
        {
            return;
        }
        
        OnBeDamage(paramModel);
    }
    protected virtual void OnBeDamage(MomentParamModel paramModel) {}
    
    public void TryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (!CanEffect())
        {
            return;
        }

        OnTryStoreBattleKey(keyType, ref count);
    }
    protected virtual void OnTryStoreBattleKey(BattleKeyType keyType, ref int count) {}
    
    #endregion

    public void Recycle()
    {
        foreach (var disposable in _registerDisposables)
        {
            disposable.Dispose();
        }
        
        _registerDisposables.Clear();
        
        OnRecycle();
    }

    protected virtual void OnRecycle() {}

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
    
    protected void EnqueueViewModel(int entityID, MomentViewType viewType, params float[] values)
    {
        EnqueueViewModel(AllocViewModel(entityID, viewType, values)); 
    }
}
