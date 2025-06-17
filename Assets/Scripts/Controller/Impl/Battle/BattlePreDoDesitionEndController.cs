using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

/// <summary>
/// 预先行动结束后 计算息 触发一些扳机 然后调用开始一轮息的计算
/// </summary>
public class BattlePreDoDesitionEndController : ControllerBase<BattlePreDoDesitionEndEventModel>
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    public override void Handle(BattlePreDoDesitionEndEventModel model)
    {
        var calculateActionWheelNormal = GameConst.Battle.CalculateActionWheelNormal;
        var aliveUnit = BattleManager.GetAllAliveUnit();
        
        //在预先行动决定后调用所有角色决定行动后的扳机
        var setUnitSkillEventModel = PoolManager.GetClass<BattleSetUnitSkillEventModel>();
        setUnitSkillEventModel.SetSkillUnitList = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue()
            .Select(behaviour => BattleManager.GetUnit(behaviour.SubjectID).EntityID).ToList();
        MessageManager.Dispatch(setUnitSkillEventModel);
        PoolManager.RecycleClass(setUnitSkillEventModel);
        
        var maxSpeed = aliveUnit.Max(unit => unit.GetProperty("speed"));
        foreach (var unit in aliveUnit)
        {
            var speed = unit.GetProperty("Speed");
            var keyCount = unit.GetKeyCount;
            unit.SpeedCounting = speed + Mathf.RoundToInt(keyCount * maxSpeed * GameConst.Battle.CalculateSpeedOffset);
        }
        //计算息
        var speedCountMax = aliveUnit.Max(unit => unit.SpeedCounting);
        var speedCountMin = aliveUnit.Min(unit => unit.SpeedCounting);
        var speedCountDelta = (speedCountMax - speedCountMin) / calculateActionWheelNormal;
        foreach (var unit in aliveUnit)//计算所在息
        {
            for (int wheel = 1; wheel <= calculateActionWheelNormal; wheel++)
            {
                if (unit.SpeedCounting >= (speedCountMax - wheel * speedCountDelta))
                {
                    unit.ActionWheel = wheel;
                    break;
                }
            }
        }
        //所有活着的人调用改变息的扳机
        foreach (var unit in aliveUnit)
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.CalculateActionWheel();
            }
        }

        //在预先行动决定后调用所有角色决定行动后的扳机
        var triggerDoDesitionMomentEventModel = PoolManager.GetClass<BattleTriggerDoDesitionMomentEventModel>();
        triggerDoDesitionMomentEventModel.DoDesitionUnitList = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue()
            .Select(behaviour => BattleManager.GetUnit(behaviour.SubjectID).EntityID).ToList();
        MessageManager.Dispatch(triggerDoDesitionMomentEventModel);
        PoolManager.RecycleClass(triggerDoDesitionMomentEventModel);
        
        BattleLogicStateManager.StartOneActionWheelCalculate();
    }
}
