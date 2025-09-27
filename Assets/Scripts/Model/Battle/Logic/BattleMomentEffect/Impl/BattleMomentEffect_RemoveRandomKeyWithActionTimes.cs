using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveRandomKeyWithActionTimes : BattleMomentEffect
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var count = Math.Min(target.ActionTimes, Config.ParamList[1].ToInt());
            target.RemoveRandomKey(count);
        }
    }
}