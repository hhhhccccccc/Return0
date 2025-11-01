using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleField : IModel
{
    [Inject] private IMessageManager MessageManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }

    public int Uid { get; set; }
    private BattlePlayerData Data { get; set; }

    public Dictionary<int, BattleRole> GetBattleUnitDict() => BattleRole;
    private Dictionary<int, BattleRole> BattleRole = new();
    
    #region 队伍道具
    private DictAndList<int, BattleProp> PropDic = new();
    public List<BattleProp> GetTeamProp() => PropDic.GetListValue();
    public int ReduceProp(int itemID, int itemCount)
    {
        var propModel = PropDic.TryGetValue(itemID);
        if (propModel == null)
        {
            return 0;
        }

        if (propModel.Count > itemCount)
        {
            propModel.Count -= itemCount;
            return itemCount;
        }
        else
        {
            var reduceCount = propModel.Count;
            propModel.Count = 0;
            PropDic.Remove(itemID);
            PoolManager.RecycleClass(propModel);
            return reduceCount;
        }
    }

    public void AddProp(int itemID, int count)
    {
        var propModel = PropDic.TryGetValue(itemID);
        if (propModel == null)
        {
            propModel = PoolManager.GetClass<BattleProp>();
            propModel.ItemID = itemID;
            propModel.Count = count;
            PropDic.Add(itemID, propModel);
        }

        propModel.Count += count;
    }
    #endregion
    
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