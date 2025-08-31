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
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    
    public override void Handle(BattleOneActionWheelEndEventModel model)
    {
        BattleLogicStateManager.SetAfterStartActionWheel(false);
    }
}
