using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10063 : BattleHeartMethodBase
{
    private HashSet<int> SkillSet = new();
 
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var skill = Subject.GetSkill();
        if (skill != null)
        {
            var faction = BattleUtil.GetSkillFactionID(skill.SkillID);
            if (faction == 0 && !SkillSet.Contains(skill.SkillID))
            {
                Subject.AddRandomKey(GetParamInt(0), ChangeKeyReason.HeartMethodEffect);
                SkillSet.Add(skill.SkillID);
            }
        }
    }

    public override void Recycle()
    {
        SkillSet.Clear();
        base.Recycle();
    }
}