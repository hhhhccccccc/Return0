using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1051 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        //获得1个随机的键
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}