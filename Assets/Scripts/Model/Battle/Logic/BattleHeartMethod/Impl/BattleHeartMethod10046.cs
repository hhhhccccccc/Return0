using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10046 : BattleHeartMethodBase
{
    private float SkillWelly => GetConfigParamFloat(0);
    public override float GetWellyRateEx(int skillGuid)
    {
        var list = Subject.PreUseSkillDataManager.GetSkillPreUseKeyCost(skillGuid);
        if (list == null)
        {
            return 0;
        }

        if (list.Count(o => o == (int)BattleKeyType.KeyUp) >= 2)
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
            return SkillWelly;
        }

        return 0;
    }
}