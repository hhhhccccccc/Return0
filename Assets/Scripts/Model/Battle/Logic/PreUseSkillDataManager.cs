using System;
using System.Collections.Generic;
using cfg;
using Zenject;

/// <summary>
/// 这里的方法用作技能本身使用产生的额外效果
/// </summary>
public class PreUseSkillDataManager : IModel, IRecycle
{
    private static Dictionary<string, Type> SkillPreUseDataNameToType { get; } = new();
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }

    /// <summary>
    /// Guid => SkillPreUseDataBase
    /// </summary>
    private Dictionary<int, SkillPreUseDataBase> SkillPreUseDataDict { get; } = new();
    
    public void TryAddSkillPreUseData(int skillGuid)
    {
        if (!SkillPreUseDataDict.TryGetValue(skillGuid, out var data))
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            var useDataScript = config.SkillPreUseDataScript;
            if (string.IsNullOrEmpty(useDataScript))
            {
                data = PoolManager.GetClass<SkillPreUseDataBase>();
                data.SkillID = skillID;
                data.VariantID = variantID;
                data.UseCount = 0;
                data.LastUseSkillStateStack = new Stack<LastUseSkillState>();
            }
            else
            {
                if (!SkillPreUseDataNameToType.TryGetValue(useDataScript, out var type))
                {
                    type = Type.GetType(useDataScript);
                    SkillPreUseDataNameToType.Add(useDataScript, type);
                }

                data = (SkillPreUseDataBase)PoolManager.GetClass(type);
                data.SkillID = skillID;
                data.VariantID = variantID;
                data.UseCount = 0;
                data.LastUseSkillStateStack = new Stack<LastUseSkillState>();
            }
           
            SkillPreUseDataDict.Add(skillGuid, data);
        }
    }
    
    public void TryAddSkillPreUseDataBySkillEnd(int skillGuid, LastUseSkillState useState)
    {
        if (SkillPreUseDataDict.TryGetValue(skillGuid, out var data))
        {
            data.UseCount += 1;
            data.LastUseSkillStateStack.Push(useState);
        }
    }
    
    private SkillPreUseDataBase GetSkillPreUseData(int skillGuid)
    {
        if (SkillPreUseDataDict.TryGetValue(skillGuid, out var data))
        {
            return data;
        }

        return null;
    }

    public int GetSkillUseCount(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            return 0;
        }

        return preData.UseCount;
    }
    
    public float GetSkillID(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.Id;
        }

        return preData.SkillID;
    }

    public float GetSkillPreUseDamage(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.Damage;
        }

        return preData.GetDamage();
    }
    
    public float GetSkillPreUseGangQiCost(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.GangQiCost;
        }

        return preData.GetGangQiCost();
    }
    
    public float GetSkillPreUseXuanQiCost(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.XuanQiCost;
        }

        return preData.GetXuanQiCost();
    }

    public List<int> GetSkillPreUseKeyCost(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.NeedKey;
        }

        return preData.GetKeyCost();
    }
    
    public SkillType GetSkillPreUseSkillType(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return (SkillType)config.SkillType;
        }

        return preData.GetSkillType();
    }
    
    public DamageType GetSkillPreUseDamageType(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return (DamageType)config.DamageType;
        }

        return preData.GetDamageType();
    }

    public LastUseSkillState GetLastUseSkillState(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            return LastUseSkillState.None;
        }

        return preData.GetLastUseSkillState();
    }

    public float GetSkillWellyEffect(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.SkillWellyEffect;
        }

        return preData.GetSkillWellyEffect();
    }
    
    public float GetSkillArmorPiercing(int skillGuid)
    {
        var preData = GetSkillPreUseData(skillGuid);
        if (preData == null)
        {
            var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.SkillArmorPiercing;
        }

        return preData.GetSkillArmorPiercing();
    }
    

    public void Recycle()
    {
        foreach (var kv in SkillPreUseDataDict)
        {
            PoolManager.RecycleClass(kv.Value);
        }
        
        SkillPreUseDataDict.Clear();
    }
}
