using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckCurrActionWheel : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    protected override bool OnCondition()
    {
        var check = Config.ParamList[0].ToInt();
        var state = Config.ParamList[1].ToInt() == 1;
        return (BattleLogicStateManager.ActionWheel == check && state) || (BattleLogicStateManager.ActionWheel != check && !state);
    }
}