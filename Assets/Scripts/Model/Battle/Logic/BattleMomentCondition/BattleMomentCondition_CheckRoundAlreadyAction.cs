using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckRoundAlreadyAction : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0].ToInt());
        if (target != null)
        {
            return BattleLogicStateManager.UnitIsRoundAlreadyAction(target.EntityID) == (Config.ParamList[1].ToInt() == 1);
        }

        return false;
    }
}