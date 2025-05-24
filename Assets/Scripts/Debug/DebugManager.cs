using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class DebugManager : MonoSingleton<DebugManager>
{
    [LabelText("调试配置")] 
    private DebugConfig DebugConfig;
    
    private DiContainer DiContainer;
    private BattleManager BattleManager;
    private IResourceManager ResourceManager;
    private IPoolManager PoolManager;
    private ILogManager LogManager;
    public void DebugStart(DiContainer diContainer)
    {
        DiContainer = diContainer;
        BattleManager = diContainer.Resolve<BattleManager>();
        ResourceManager = diContainer.Resolve<IResourceManager>();
        PoolManager = diContainer.Resolve<IPoolManager>();
        LogManager = diContainer.Resolve<ILogManager>();
        LogManager.Debug("调试开战初始化");
        InitDebugData();
    }

    private List<PlayerData> GenerateDebugPlayer()
    {
        var data = new List<PlayerData>();
        foreach (var debugData in DebugConfig.Players)
        {
            var playerData = PoolManager.GetClass<PlayerData>();
            playerData.Uid = debugData.Uid;
            foreach (var debugInfo in debugData.Characters)
            {
                var character = PoolManager.GetClass<Character>();
                character.ID = debugInfo.ID;
                character.SlotIndex = debugInfo.SlotIndex;
                character.CharacterID = debugInfo.CharacterID;
                character.Hp = debugInfo.Hp;
                character.Speed = debugInfo.Speed;
                character.Strength = debugInfo.Strength;
                character.Prevent = debugInfo.Prevent;
                character.Technique = debugInfo.Technique;
                character.Resist = debugInfo.Resist;
                character.Clever = debugInfo.Clever;
                character.GangQi = debugInfo.GangQi;
                character.XuanQi = debugInfo.XuanQi;
                playerData.Characters.Add(character);
            }
            data.Add(playerData);
        }
        
        return data;
    }

    private void InitDebugData()
    {
        DebugConfig = ResourceManager.Load<DebugConfig>("Assets/Scripts/Debug/DebugConfig.asset");
        BattleManager.BattleInit(GenerateDebugPlayer());
    }
}
