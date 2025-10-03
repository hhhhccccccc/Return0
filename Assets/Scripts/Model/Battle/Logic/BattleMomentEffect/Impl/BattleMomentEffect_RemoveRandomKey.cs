using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveRandomKey : BattleMomentEffect
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var count = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                target.RemoveRandomKey(count);
            }
        }
    }
}