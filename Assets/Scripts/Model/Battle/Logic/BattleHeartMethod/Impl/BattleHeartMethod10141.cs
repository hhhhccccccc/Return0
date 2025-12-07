using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10141 : BattleHeartMethodBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var skill = Subject.GetSkill();
            if (skill.GetSKillType == SkillType.PowerKilling || skill.GetSKillType == SkillType.ArtKilling)
            {
                var target = BattleManager.GetUnit(model.HitID);
                if (target != null && !target.HasBuff(GameConst.Battle.Buff20071))
                {
                    BattleBuffManager.AddBuff(target, GameConst.Battle.Buff20071, Subject, GetParamInt(0));
                }
            }
        }
    }
}