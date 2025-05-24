using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleField : IModel
{
    public int Uid;
    private PlayerData Data;

    private Dictionary<int, BattleRole> BattleRoles = new();
    [Inject] 
    private IMessageManager MessageManager;
    
    [Inject]
    private IPoolManager PoolManager;
    
    public void Init(PlayerData data)
    {
        Data = data;
        Uid = data.Uid;
        foreach (var character in Data.Characters)
        {
            var roleInfo = PoolManager.GetClass<BattleRole>();
            roleInfo.Init(this, character);
            BattleRoles.Add(character.SlotIndex, roleInfo);
        }
    }

    public BattleRole GetBattleRole(int slotIndex)
    {
        return BattleRoles.GetValueOrDefault(slotIndex, null);
    }
}