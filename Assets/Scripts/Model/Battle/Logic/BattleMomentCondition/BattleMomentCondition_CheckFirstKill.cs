using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckFirstKill : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    protected override bool OnCondition()
    {
        if (Target.IsAlive())
        {
            return false;
        }

        if (Subject.KillUnitList.Count != 1)
        {
            return false;
        }

        if (Subject.KillUnitList[0] != Target.EntityID)
        {
            return false;
        }

        return true;
    }
}