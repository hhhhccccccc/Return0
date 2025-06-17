using System;
using System.Collections.Generic;
using Zenject;

public class BattleMomentConditionManager : SingleModel
{
    [Inject] private IPoolManager PoolManager;
    [Inject] private IConfigManager ConfigManager;
    
    private Dictionary<string, Type> NameToType = new();

    public bool GetCondition(int conditionID, BattleUnit subject, BattleUnit target)
    {
        var config = ConfigManager.GetBattleMomentCondition(conditionID);
        var typeName = $"BattleMomentCondition_{config.ConditionName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentCondition)PoolManager.GetClass(type);
        var result = model.Condition(conditionID, subject, target);
        PoolManager.RecycleClass(model);
        return result;
    }
    
    public bool GetCondition(int conditionID, BattleUnit subject, BattleUnit target, BattleUnit spellcaster)
    {
        var config = ConfigManager.GetBattleMomentCondition(conditionID);
        var typeName = $"BattleMomentCondition_{config.ConditionName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentCondition)PoolManager.GetClass(type);
        var result = model.Condition(conditionID, subject, target, spellcaster);
        PoolManager.RecycleClass(model);
        return result;
    }
}
