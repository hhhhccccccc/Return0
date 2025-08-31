using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckBeActionInBeforeActionWheel : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var offset = Config.ParamList[1].ToInt();
            var includeNow = Config.ParamList[2].ToInt() == 1;
            var targetWheel = target.ActionWheel;
            var now = BattleLogicStateManager.ActionWheel;
            if (includeNow)
            {
                return now + offset >= targetWheel;
            }

            return now + offset > targetWheel;
        }
        return false;
    }
}