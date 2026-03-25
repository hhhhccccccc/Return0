using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10047 : BattleHeartMethodBase
{
    private int ChangeActionWheel => GetParamInt(0);
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return;
        }

        if (skill.GetKeyCostList.Count(o => o == (int)BattleKeyType.KeyRight) >= 2)
        {
            var model = Subject.ChangeActionWheel(ChangeActionWheel);
            EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeActionWheel, ChangeActionWheel, model.ActionWheel, model.ActionWheelOut);
        }
    }
}