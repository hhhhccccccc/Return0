using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

/// <summary>
/// 一息结束
/// </summary>
public class BattleOneActionWheelEndController : ControllerBase<BattleOneActionWheelEndEventModel>
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    
    public override void Handle(BattleOneActionWheelEndEventModel model)
    {
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            if (unit.TryCalculateNextActionWheel())
            {
                foreach (var moment in unit.BattleMomentManager.GetMoments())
                {
                    moment.CalculateActionWheel();
                    var changeActionWheel = unit.BattleMomentManager.GetChangeActionWheel();
                    unit.ChangeActionWheel(changeActionWheel, true);
                }
            }

            foreach (var moment in unit.BattleMomentManager.GetMoments())
            {
                moment.ActionWheelEnd();
            }
        }
        
        
        BattleLogicStateManager.ActionWheelEnd();
    }
}
