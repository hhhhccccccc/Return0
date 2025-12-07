using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10153 : BattleHeartMethodBase
{
    private int SkillCount { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        SkillCount = 0;
    }

    public override void SkillEnd(BattleSkillBase skill)
    {
        base.SkillEnd(skill);
        if (skill != null)
        {
            if (skill.IsRepeat)
            {
                SkillCount++;
                if (SkillCount == GetParamInt(0))
                {
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10091, Subject, GetParamInt(1));
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10101, Subject, GetParamInt(2));
                }
            }
        }
    }

    public override void Recycle()
    {
        SkillCount = 0;
        base.Recycle();
    }
}