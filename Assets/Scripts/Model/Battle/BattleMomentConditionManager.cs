using System;
using System.Collections.Generic;
using Zenject;

public class BattleMomentConditionManager : SingleModel
{
    [Inject] private IPoolManager PoolManager;
    [Inject] private ConfigManager ConfigManager;
    
    private Dictionary<string, Type> NameToType = new();

    public bool GetCondition(int conditionID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        var config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        var typeName = $"BattleMomentCondition_{config.ConditionName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentCondition)PoolManager.GetClass(type);
        var result = model.Condition(conditionID, subject, target, paramModel);
        PoolManager.RecycleClass(model);
        return result;
    }
    
    public bool GetCondition(int conditionID, BattleUnit subject, BattleUnit target, BattleUnit spellCaster, MomentParamModel paramModel, int layerCount)
    {
        var config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        var typeName = $"BattleMomentCondition_{config.ConditionName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentCondition)PoolManager.GetClass(type);
        var result = model.Condition(conditionID, subject, target, spellCaster, paramModel, layerCount);
        PoolManager.RecycleClass(model);
        return result;
    }
    
    public bool GetCondition(int conditionID, BattleUnit subject, int skillID, MomentParamModel paramModel)
    {
        var config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        var typeName = $"BattleMomentCondition_{config.ConditionName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentCondition)PoolManager.GetClass(type);
        var result = model.Condition(conditionID, subject, skillID, paramModel);
        PoolManager.RecycleClass(model);
        return result;
    }
}
