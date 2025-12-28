using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10152 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = false;
    }

    public override void RoundStart()
    {
        base.RoundStart();
        if (CanTrigger)
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10041, Subject, GetParamInt(0));
            CanTrigger = false;
        }
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var attacker = BattleManager.GetUnit(model.SelfID);
            var skill = attacker.GetSkill();
            if (skill != null && skill.GetSKillType == SkillType.PowerKilling)
            {
                CanTrigger = true;
            }
        }
    }

    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}