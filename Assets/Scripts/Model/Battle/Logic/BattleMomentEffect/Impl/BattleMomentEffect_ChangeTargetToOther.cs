using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeTargetToOther : BattleMomentEffect
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    protected override void OnEffect()
    {
        var target1List = GetUnitByParamID(Config.ParamList[0]);
        var target2List = GetUnitByParamID(Config.ParamList[1]);
        if (target1List.Count > 0 && target1List[0].IsAlive() && target2List.Count > 0 && target2List[0].IsAlive())
        {
            BattleLogicBehaviourManager.ChangeTarget(target1List[0], target2List[0]);
        }
    }
}