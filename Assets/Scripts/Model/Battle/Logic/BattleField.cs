using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleField : IModel
{
    [Inject]
    private IMessageManager MessageManager;
        
    [Inject]
    private IPoolManager PoolManager;

    public int Uid;
    private BattlePlayerData Data;

    public Dictionary<int, BattleRole> GetBattleUnitDict() => BattleRole;
    private Dictionary<int, BattleRole> BattleRole = new();
    
   
    public void Init(BattlePlayerData data)
    {
        Data = data;
        Uid = data.Uid;
        foreach (var character in Data.HeroDatas)
        {
            var roleInfo = PoolManager.GetClass<BattleRole>();
            roleInfo.Init(this, character);
            BattleRole.Add(character.SlotIndex, roleInfo);
        }
    }

    private List<BattleUnit> AliveUnitList = new();
    
    public List<BattleUnit> GetAliveUnit()
    {
        AliveUnitList.Clear();
        foreach (var (slotIndex, role) in BattleRole)
        {
            if (role.IsAlive())
            {
                AliveUnitList.Add(role);
            }
        }

        return AliveUnitList;
    }

    public void RoundStart()
    {
        foreach (var (slotIndex, role) in BattleRole)
        {
            role.RoundStart();
        }
    }

    public void RoundEnd()
    {
        foreach (var (slotIndex, role) in BattleRole)
        {
            role.RoundEnd();
        }
    }
    
    public BattleRole GetBattleRole(int slotIndex)
    {
        return BattleRole.GetValueOrDefault(slotIndex, null);
    }
}