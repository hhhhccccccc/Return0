using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Zenject;

public class DebugManager : IManager
{
    [LabelText("调试配置")] 
    //private DebugConfig DebugConfig;
    
    [Inject] private IResourceManager ResourceManager;
    [Inject] private IMessageManager MessageManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private IPoolManager PoolManager;
    public void DebugStart()
    {
        BattleManager.Init(GenerateDebugPlayer());
    }

    private List<PlayerData> GenerateDebugPlayer()
    {
        var data = new List<PlayerData>();
        /*foreach (var player in DebugConfig.Players)
        {
            var playerData = PoolManager.GetClass<PlayerData>();
            foreach (var debugInfo in player.Characters)
            {
                var character = PoolManager.GetClass<Character>();
                character.ID = debugInfo.ID;
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
        */

        return data;
    }

    public IEnumerator Init()
    {
        //DebugConfig = ResourceManager.Load<DebugConfig>("Assets/Scripts/Debug/DebugConfig.asset");
        //MessageManager.Register<ResourceLoadEndEventModel>(OnResourceLoadEnd);
        yield break;
    }
}
