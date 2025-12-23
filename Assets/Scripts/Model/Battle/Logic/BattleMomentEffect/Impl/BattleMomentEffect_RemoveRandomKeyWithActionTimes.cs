using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveRandomKeyWithActionTimes : BattleMomentEffect
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var count = Math.Min(target.ActionTimes, Config.ParamList[1].ToInt());
                target.RemoveRandomKey(count, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
            }
        }
    }
}