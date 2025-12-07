using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10046 : BattleHeartMethodBase
{
    public override float AddSkillWellyRate(int skillGuid)
    {
        var list = Subject.PreUseSkillDataManager.GetSkillPreUseKeyCost(skillGuid);
        if (list == null)
        {
            return 0;
        }

        if (list.Count(o => o == (int)BattleKeyType.KeyUp) >= 2)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}