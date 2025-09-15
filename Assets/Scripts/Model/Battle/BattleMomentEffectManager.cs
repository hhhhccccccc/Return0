using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentEffectManager : SingleModel
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private IPoolManager PoolManager;
    
    private Dictionary<string, Type> NameToType = new();

    public BattleMomentViewModel OnEffect(int momentEffectID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        var config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        var typeName = $"BattleMomentEffect_{config.EffectName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var effectModel = (BattleMomentEffect)PoolManager.GetClass(type);
        var viewModel = effectModel.Effect(momentEffectID, subject, target, paramModel);
        PoolManager.RecycleClass(effectModel);
        return viewModel;
    }
    
    public BattleMomentViewModel OnEffect(int momentEffectID, BattleUnit subject, BattleUnit target, BattleUnit spellCaster, MomentParamModel paramModel, int layerCount)
    {
        var config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        var typeName = $"BattleMomentEffect_{config.EffectName}";
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var effectModel = (BattleMomentEffect)PoolManager.GetClass(type);
        var viewModel = effectModel.Effect(momentEffectID, subject, target, spellCaster, paramModel, layerCount);
        PoolManager.RecycleClass(effectModel);
        return viewModel;
    }
}
