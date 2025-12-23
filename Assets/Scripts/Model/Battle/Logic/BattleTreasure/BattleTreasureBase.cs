using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel, IGetBattlePropertyChanged, IRecycle
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    public int TreasureID { get; set; }
    public BattleUnit Subject { get; set; }
    public TreasureConfig Config { get; set; }
    public void Init(int treasureID, BattleUnit subject)
    {
        TreasureID = treasureID;
        Subject = subject;
        Config = ConfigManager.GetTreasureConfig(treasureID);
        InitMoment(this);
    }

    private bool CanTrigger()
    {
        var aliveUnitList = BattleManager.GetAllAliveUnit();
        if (aliveUnitList.Any(unit => unit.BattleChangeModelManager.CheckHasMethod(GameConst.Battle.HeartMethod10095)))
        {
            return false;
        }

        return true;
    }
        
    #region 战斗改变属性机制

    
    public float AddSkillWellyRate(int skillGuid)
    {
        if (!CanTrigger())
        {
            return 0;
        }
        
        return 0;
    }

    public float AddSkillWellyEffect(int skillGuid)
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

        return 0;
    }
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

    public float AddSkillDamageRate(int skillGuid)
    {
        if (!CanTrigger())
        {
            return 0;
        }

        return 0;
    }
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

    public void ChangeDamageValue(Dictionary<int, float> dict, MomentParamModel paramModel)
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
        
        return true;
    }

    public bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return false;
        }
        
        return false;
    }

    public bool CanBeCounter(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return true;
        }
        return true;
    }

    #endregion

    public virtual void Recycle()
    {
        
    }
}
