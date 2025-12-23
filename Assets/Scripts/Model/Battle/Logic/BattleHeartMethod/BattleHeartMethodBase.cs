using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleHeartMethodMoment, IModel, IGetBattlePropertyChanged, IRecycle
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
    
    [Inject] protected IMessageManager MessageManager { get; set; }
    [Inject] protected ConfigHelper ConfigHelper { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] protected BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] protected BattleUtil BattleUtil { get; set; }
    public int HeartMethodID { get; set; }
    public HeartMethodConfig Config { get; set; }
    public BattleUnit Subject { get; set; }
    protected float GetParamFloat(int index) => Config.ParamEx[index];
    public int GetParamInt(int index) => Config.ParamEx[index].ToInt();
    
    public virtual void Init(int heartMethodID, BattleUnit subject)
    {
        HeartMethodID = heartMethodID;
        Config = ConfigManager.GetHeartMethodConfig(HeartMethodID);
        Subject = subject;
        InitMoment(this);
    }

    #region 战斗改变属性机制
    public virtual float AddSkillWellyRate(int skillGuid) => 0;
    public float AddSkillWellyEffect(int skillGuid) => 0;
    public void TrySetBaseWellyRate(int skillGuid, ref float value)
    {
        
    }

    public void TrySetAddWellyRate(int skillGuid, ref float value)
    {
        
    }

    public int GetKeyMaxEx() => 0;
    public virtual void HpChanged()
    {
        
    }

    public virtual void SkillEnd(BattleSkillBase skillBase)
    {
        
    }

    public virtual float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null) => 0;
    public virtual void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        
    }

    public virtual int GetChangeActionWheel() => 0;
    public virtual float AddSkillDamageRate(int skillGuid) => 0;
    public virtual void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public virtual void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public virtual void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public virtual void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        
    }

    public virtual float GetReplaceSkillGangQiCost() => 0;
    public virtual void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        
    }

    public virtual float GetReplaceSkillXuanQiCost() => 0;
    public virtual void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        
    }

    public virtual void OnKillUnit(int beKillID)
    {
        
    }

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost) => (gangQiCost, xuanQiCost);
    public virtual bool CheckReCalculateDamage(MomentParamModel model)
    {
        return false;
    }

    public virtual void BeforeReduceHp(float reduceHp)
    {
        
    }

    public virtual void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        
    }

    public virtual void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        
    }

    public virtual void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        
    }

    public virtual void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        
    }

    public virtual void EndAction()
    {
        
    }

    public virtual void RemoveBeforeNextAction()
    {
        
    }

    public virtual void BuffLayerCountChanged(int buffID, int layerCount)
    {
        
    }

    public virtual void ChangeDamageValue(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        
    }

    public virtual void AfterUnitInit()
    {
        
    }

    public virtual void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        
    }

    public virtual void BeCounter()
    {
        
    }

    public virtual void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        
    }

    public virtual bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        return true;
    }

    public virtual bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        return false;
    }

    public virtual bool CanBeCounter(MomentParamModel paramModel)
    {
        return true;
    }

    #endregion

    public virtual void Recycle()
    {
        
    }
}

