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

    
    public float GetSkillWellyRate(int skillGuid)
    {
        if (!CanTrigger())
        {
            return 0;
        }
        
        return OnGetSkillWellyRate(skillGuid);
    }

    protected virtual float OnGetSkillWellyRate(int skillGuid) => 0;

    public float GetSkillWellyEffect(int skillGuid)
    {
        if (!CanTrigger())
        {
            return 0;
        }
        
        return 0;
    }

    public void TrySetBaseWellyRate(int skillGuid, ref float value)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void TrySetAddWellyRate(int skillGuid, ref float value)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public int GetKeyMaxEx()
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return 0;
    }
    public void HpChanged()
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void SkillEnd(BattleSkillBase skill)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!CanTrigger())
        {
            return 0;
        } 

        return OnGetProperty(propertyType, model);
    }

    public virtual float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null) => 0;
    
    public void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public int GetChangeActionWheel()
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return 0;
    }

    public float GetSkillDamageRate(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return OnGetSkillDamageRate(paramModel);
    }
    protected virtual float OnGetSkillDamageRate(MomentParamModel paramModel) => 0;
    
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public virtual void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public float GetReplaceSkillGangQiCost()
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return 0;
    }
    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public float GetReplaceSkillXuanQiCost()
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return 0;
    }
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public virtual void OnKillUnit(int beKillID)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        if (!CanTrigger())
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
        if (!CanTrigger())
        {
            return;
        }
    }

    public void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public virtual void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public virtual void EndAction()
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void RemoveBeforeNextAction()
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (!CanTrigger())
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
        if (!CanTrigger())
        {
            return;
        }
    }

    public void AfterUnitInit()
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void BeCounter()
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        if (!CanTrigger())
        {
            return;
        }
    }

    public bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        if (!CanTrigger())
        {
            return true;
        }

        return OnCheckCanAddBuff(buffID, ref addCount, spellCasterID, momentType);
    }

    protected virtual bool OnCheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType = BattleMomentType.None) => true;

    public bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return false;
        }
        
        return OnCanIgnoreSkillDirectDamage(paramModel);
    }
    protected virtual bool OnCanIgnoreSkillDirectDamage(MomentParamModel paramModel) => false;

    public bool CanBeCounter(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return true;
        }
        return true;
    }

    public float GetDamageReducePct(int attackID, DamageType damageType)
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return OnGetDamageReducePct(attackID, damageType);
    }
    protected virtual float OnGetDamageReducePct(int attackID, DamageType damageType) => 0;

    public void BeforeAttack(MomentParamModel model)
    {
        if (!CanTrigger())
        {
            return;
        }
        
        OnBeforeAttack(model);
    }
    protected virtual void OnBeforeAttack(MomentParamModel model) {}

    public void BeDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        
        OnBeDamage(paramModel);
    }
    protected virtual void OnBeDamage(MomentParamModel paramModel) {}
    
    public void TryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (!CanTrigger())
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
}
