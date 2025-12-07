using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10133 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff20341, Subject, GetParamInt(0));
        var buffCount = Subject.GetBuffCountByID(GameConst.Battle.Buff20341);
        if (buffCount > GetParamInt(1))
        {
            Subject.AddActionTimes(GetParamInt(2));
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var skill = Subject.GetSkill();
        if (skill != null && paramModel is DamageParamModel model)
        {
            if (skill.GetSKillType == SkillType.PowerKilling || skill.GetSKillType == SkillType.ArtKilling)
            {
                var target = BattleManager.GetUnit(model.HitID);
                BattleBuffManager.AddBuff(target, GameConst.Battle.Buff20341, Subject, GetParamInt(3));
            }

            if (skill.GetSKillType == SkillType.TechniqueImperialStyle)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff20341, Subject, GetParamInt(4));
            }
        }
    }
}