using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BattleManager : SingleModel
{
    #region 逻辑Unit的字典
    
    private int CurrentEntityID;
    private Dictionary<int, BattleUnit> UnitDict;
    public void ResetUnitToDict(BattleUnit unit)
    {
        CurrentEntityID++;
        unit.EntityID = CurrentEntityID;
        UnitDict.Add(unit.EntityID, unit);
    }

    public BattleUnit GetUnit(int entityID)
    {
        return UnitDict.GetValueOrDefault(entityID, null);
    }

    #endregion

    #region Inject注入

    [Inject] private IPoolManager PoolManager;
    [Inject] private ILogManager LogManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;

    #endregion

    #region 战斗数据
    
    public List<BattleField> BfList;

    public BattleField SelfBf;
    public BattleField OtherBf;

    #endregion

    
    private void DataInit()
    {
        CurrentEntityID = 0;
        BfList = new List<BattleField>();
        UnitDict = new Dictionary<int, BattleUnit>();
    }
    
    public void BattleInit(List<PlayerData> players)
    {
        BattleDataManager.SetPlayerData(players);
        DataInit();   
        foreach (var playerData in players)
        {
            var bf = PoolManager.GetClass<BattleField>();
            bf.Init(playerData);
            BfList.Add(bf);
            if (playerData.Uid == 1)
            {
                SelfBf = bf;
            }
            else
            {
                OtherBf = bf;
            }
        }

        MessageManager.DispatchMsg<BattleLogicReadyEventModel>(null);
    }
    
    public void BattleStart()
    {
        LogManager.Debug("[战斗开始]");

        foreach (var unit in GetAllAliveUnit())
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.BattleStart();
            }
        }
        
        MessageManager.DispatchMsg<BattleRoundStartEventModel>(null);
    }

    public void RoundStart()
    {
        
    }
    
    public void RoundEnd()
    {
        
    }

    public List<BattleUnit> GetSelfBfAliveUnit() => SelfBf.GetAliveUnit();
    private List<BattleUnit> TempAllBattleUnitList = new();
    public List<BattleUnit> GetAllAliveUnit()
    {
        TempAllBattleUnitList.Clear();
        foreach (var bf in BfList)
        {
            TempAllBattleUnitList.AddRange(bf.GetAliveUnit());
        }

        return TempAllBattleUnitList;
    }

    /// <summary>
    /// 获取当前息可以做决定行动的角色
    /// </summary>
    /// <returns></returns>
    public List<int> GetCurrActionWheelCanDoDesitionUnit()
    {
        var result = new List<int>();
        var aliveUnit = GetSelfBfAliveUnit();
        var allBehaviour = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue();
        foreach (var unit in aliveUnit)
        {
            //当前息可以行动  指令列表中没有该角色  行动次数大于0
            if (BattleLogicStateManager.ActionWheel >= unit.ActionWheel && allBehaviour.All(behaviour => behaviour.SubjectID != unit.EntityID) && unit.ActionTimes > 0)
            {
                result.Add(unit.EntityID);
            }
        }

        return result;
    }
    
    /// <summary>
    /// 获取当前息行动的角色
    /// </summary>
    /// <returns></returns>
    public List<int> GetCurrActionWheelUnit()
    {
        var result = new List<int>();
        var aliveUnit = GetAllAliveUnit();
        var allBehaviour = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue();
        foreach (var unit in aliveUnit)
        {
            //当前息可以行动  指令列表中没有该角色  行动次数大于0
            if (unit.ActionWheel == BattleLogicStateManager.ActionWheel && allBehaviour.Any(behaviour => behaviour.SubjectID == unit.EntityID) && unit.ActionTimes > 0)
            {
                result.Add(unit.EntityID);
            }
        }

        return result;
    }

    public bool CheckIsSelfUnit(int entityID) => GetUnit(entityID).IsSelf;
}