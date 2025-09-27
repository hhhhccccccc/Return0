using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckCurrActionWheel : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    protected override bool OnCondition()
    {
        var check = Config.ParamList[0].ToInt();
        return BattleLogicStateManager.ActionWheel == check;
    }
}