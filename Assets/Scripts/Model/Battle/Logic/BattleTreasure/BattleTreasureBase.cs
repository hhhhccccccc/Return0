using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel, IGetBattlePropertyChanged, IRecycle
{
    public int TreasureID;

    public BattleUnit Subject;
    
    public TreasureConfig Config;

    [Inject] private ConfigManager ConfigManager;

    public void Init(int treasureID, BattleUnit subject)
    {
        TreasureID = treasureID;
        Subject = subject;
        Config = ConfigManager.GetTreasureConfig(treasureID);
        InitMoment(this);
    }
    
    #region 战斗改变属性机制

    public float AddSkillWellyRate(int skillGuid)
    {
        return 0;
    }

    public float AddSkillWellyEffect(int skillGuid)
    {
        return 0;
    }

    public void TrySetBaseWellyRate(int skillGuid, ref float value)
    {
        
    }

    public void TrySetAddWellyRate(int skillGuid, ref float value)
    {
        
    }

    public int GetKeyMaxEx() => 0;
    public void HpChanged()
    {
        
    }

    public void SkillEnd(BattleSkillBase skill)
    {
        
    }

    public float GetProperty(BattlePropertyType propertyType) => 0;
    public int GetChangeActionWheel() => 0;
    public float AddSkillDamageRate(int skillGuid) => 0;
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        
    }

    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        
    }

    public void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        
    }

    public float GetReplaceSkillGangQiCost() => 0;
    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        
    }

    public float GetReplaceSkillXuanQiCost() => 0;
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        
    }

    public virtual void OnKillUnit(int beKillID)
    {
        
    }

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost) => (gangQiCost, xuanQiCost);

    public void BeforeReduceHp(float reduceHp)
    {
        
    }

    public void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        
    }

    public void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        
    }

    public void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        
    }

    public virtual void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        
    }

    public virtual void EndAction()
    {
        
    }

    public void RemoveBeforeNextAction()
    {
        
    }

    public void BuffLayerCountChanged(int buffID, int layerCount)
    {
        
    }

    #endregion

    public virtual void Recycle()
    {
        
    }
}
