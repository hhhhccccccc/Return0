using System;
using System.Collections.Generic;
using cfg;
using Zenject;

/// <summary>
/// 这里的方法用作技能本身使用产生的额外效果
/// </summary>
public class PreUseSkillDataManager : IModel, IRecycle
{
    private static Dictionary<string, Type> SkillPreUseDataNameToType = new();
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }

    private Dictionary<int, SkillPreUseDataBase> SkillPreUseDataDict = new();
    
    public void TryAddSkillPreUseData(int skillID)
    {
        if (!SkillPreUseDataDict.TryGetValue(skillID, out var data))
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            var useDataScript = config.SkillPreUseDataScript;
            if (string.IsNullOrEmpty(useDataScript))
            {
                data = PoolManager.GetClass<SkillPreUseDataBase>();
                data.SkillID = skillID;
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
                data.UseCount = 0;
                data.LastUseSkillStateStack = new Stack<LastUseSkillState>();
            }
           
            SkillPreUseDataDict.Add(skillID, data);
        }
    }
    
    public void TryAddSkillPreUseDataBySkillEnd(int skillID, LastUseSkillState useState)
    {
        if (SkillPreUseDataDict.TryGetValue(skillID, out var data))
        {
            data.UseCount += 1;
            data.LastUseSkillStateStack.Push(useState);
        }
    }
    
    private SkillPreUseDataBase GetSkillPreUseData(int skillID)
    {
        if (SkillPreUseDataDict.TryGetValue(skillID, out var data))
        {
            return data;
        }

        return null;
    }

    public int GetSkillUseCount(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            return 0;
        }

        return preData.UseCount;
    }
    
    public float GetSkillID(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.Id;
        }

        return preData.SkillID;
    }

    public float GetSkillPreUseDamage(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.Damage;
        }

        return preData.GetDamage();
    }
    
    public float GetSkillPreUseGangQiCost(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.GangQiCost;
        }

        return preData.GetGangQiCost();
    }
    
    public float GetSkillPreUseXuanQiCost(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.XuanQiCost;
        }

        return preData.GetXuanQiCost();
    }

    public List<int> GetSkillPreUseKeyCost(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.NeedKey;
        }

        return preData.GetKeyCost();
    }
    
    public SkillType GetSkillPreUseSkillType(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return (SkillType)config.SkillType;
        }

        return preData.GetSkillType();
    }
    
    public DamageType GetSkillPreUseDamageType(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return (DamageType)config.DamageType;
        }

        return preData.GetDamageType();
    }

    public LastUseSkillState GetLastUseSkillState(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            return LastUseSkillState.None;
        }

        return preData.GetLastUseSkillState();
    }

    public float GetSkillDamageEffectDelta(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return config.SkillDamageEffectDelta;
        }

        return preData.GetSkillDamageEffectDelta();
    }
    
    public float GetSkillArmorPiercing(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
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
