using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1051 : BattleSkillBase
{
    //获得1个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}