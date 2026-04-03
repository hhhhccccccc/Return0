using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2052 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var count = Math.Min(Target.ActionTimes, 2);
        DoRemoveRandomKey(Target, count, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
}