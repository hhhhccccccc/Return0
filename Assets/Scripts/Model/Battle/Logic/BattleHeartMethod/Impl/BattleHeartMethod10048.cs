using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10048 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.CheckClashIsWin(Subject.EntityID))
            {
                var skill = Subject.GetSkill();
                if (skill != null)
                {
                    var target = skill.Target;
                    DoRemoveRandomKey(target, GetConfigParamInt(0), ChangeKeyReason.HeartMethodEffect, ChangeKeyType.Remove);
                }
            }
        }
    }
}