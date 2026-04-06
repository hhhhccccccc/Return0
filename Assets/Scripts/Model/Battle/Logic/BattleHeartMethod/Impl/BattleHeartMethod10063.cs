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
                SkillSet.Add(skill.SkillID);
                DoAddRandomKey(Subject, GetConfigParamInt(0), ChangeKeyReason.HeartMethodEffect);
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        SkillSet.Clear();
    }
}