using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

/// <summary>
/// 预先行动结束后 计算息 触发一些扳机 然后调用开始一轮息的计算
/// </summary>
public class BattlePreCalculateUnitActionWheelController : ControllerBase<BattlePreCalculateUnitActionWheelEventModel>
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    public override void Handle(BattlePreCalculateUnitActionWheelEventModel model)
    {
        var calculateActionWheelNormal = GameConst.Battle.CalculateActionWheelNormal;
        var aliveUnit = BattleManager.GetAllAliveUnit();
        var maxSpeed = aliveUnit.Max(unit => unit.GetProperty(BattlePropertyType.Speed));
        foreach (var unit in aliveUnit)
        {
            var speed = unit.GetProperty(BattlePropertyType.Speed);
            var keyCount = unit.GetAllKeyCount();
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

        foreach (var unit in aliveUnit)
        {
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.CalculateActionWheel();
            }
        }
    }
}
