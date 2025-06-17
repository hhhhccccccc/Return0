using System;
using System.Collections.Generic;
using Zenject;

public class BattleMomentEffectManager : SingleModel
{
    [Inject] private IConfigManager ConfigManager;
    [Inject] private IPoolManager PoolManager;
    
    private Dictionary<string, Type> NameToType = new();

    public void OnEffect(int momentEffectID, BattleUnit subject, BattleUnit target)
    {
        var config = ConfigManager.GetBattleMomentEffect(momentEffectID);
        var typeName = $"BattleMomentCondition_{config.EffectName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentEffect)PoolManager.GetClass(type);
        model.Effect(momentEffectID, subject, target);
        PoolManager.RecycleClass(model);
    }
    
    public void OnEffect(int momentEffectID, BattleUnit subject, BattleUnit target, BattleUnit spellcaster)
    {
        var config = ConfigManager.GetBattleMomentEffect(momentEffectID);
        var typeName = $"BattleMomentCondition_{config.EffectName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var model = (BattleMomentEffect)PoolManager.GetClass(type);
        model.Effect(momentEffectID, subject, target, spellcaster);
        PoolManager.RecycleClass(model);
    }
}
