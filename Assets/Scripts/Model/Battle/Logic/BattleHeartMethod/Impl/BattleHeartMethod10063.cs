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
                var addKeyList = Subject.AddRandomKey(GetConfigParamInt(0), ChangeKeyReason.HeartMethodEffect);
                if (addKeyList is { Count: > 0 })
                {
                    var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey, Subject.EntityID);
                    viewModel.AddKeyList(addKeyList);
                    EnqueueViewModel(viewModel);
                }
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        SkillSet.Clear();
    }
}