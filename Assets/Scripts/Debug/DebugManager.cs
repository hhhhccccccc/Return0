using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<BattlePlayerData> GenerateDebugPlayer()
    {
        var data = new List<BattlePlayerData>();
        foreach (var debugData in DebugConfig.Players)
        {
            var playerData = PoolManager.GetClass<BattlePlayerData>();
            playerData.Uid = debugData.Uid;
            foreach (var debugHero in debugData.HeroDatas)
            {
                var heroData = PoolManager.GetClass<HeroData>();
                heroData.Init(debugHero.HeroID, debugHero.Level);
                heroData.SetSlotIndex(debugHero.SlotIndex);
                heroData.SetWearSkill(debugHero.WearSkill);
                heroData.SetHeartMethod(debugHero.WearHeartMethod);
                heroData.SetWearTreasure(debugHero.WearTreasure);
                playerData.HeroDatas.Add(heroData);
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
