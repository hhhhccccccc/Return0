using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

//todo 表现
public class BattleHeartMethod10011 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(0);
    private bool CanTrigger { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = false;
    }

    public override void RoundStart()
    {
        CanTrigger = true;
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        if (!CanTrigger)
        {
            return;
        }

        if (paramModel is DamageParamModel model)
        {
            var useSuccess = model.GetSelfSkillUseSuccess(Subject.EntityID);
            var skillID = model.GetSelfSkillID(Subject.EntityID);
            if (useSuccess && GameConst.Battle.UseItemSkillIDList.Contains(skillID))
            {
                Subject.AddActionTimes(Times);
                CanTrigger = false;
                EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}