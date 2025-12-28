using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10129 : BattleHeartMethodBase
{
    private bool NeedSuccess { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        NeedSuccess = true;
    }

    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var state = model.GetSelfClashState(Subject.EntityID);
            if ((state && NeedSuccess) || (!state && !NeedSuccess))
            {
                var commonPool = ConfigHelper.RandomCommonPool(GetParamInt(0));
                BattleBuffManager.AddBuff(Subject, commonPool[0].ID, Subject, commonPool[0].Num);
            }
        }
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        NeedSuccess = !NeedSuccess;
    }

    protected override void OnRecycle()
    {
        NeedSuccess = false;
    }
}