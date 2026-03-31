using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10032 : BattleHeartMethodBase
{
    public override float GetWellyRateEx(int skillGuid)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(s) != SkillType.PowerKilling)
        {
            return 0;
        }

        var buff = Subject.GetBuff(GameConst.Battle.BuffJiaoMing);
        if (buff == null)
        {
            return 0;
        }

        var welly = buff.LayerCount * GetParamFloat(0);
        EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, welly);
        return welly;
    }
}