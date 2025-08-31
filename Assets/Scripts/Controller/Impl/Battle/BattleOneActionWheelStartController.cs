using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

/// <summary>
/// 一息开始
/// </summary>
public class BattleOneActionWheelStartController : ControllerBase<BattleOneActionWheelStartEventModel>
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    
    public override void Handle(BattleOneActionWheelStartEventModel model)
    {
        BattleLogicStateManager.StartOneActionWheelCalculate();
    }
}
