using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10047 : BattleHeartMethodBase
{
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
            Subject.ChangeActionWheel(GetParamInt(0));
        }
    }
}