using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3029 : BattleSkillBase
{
    //随机减少目标2个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoRemoveRandomKey(Target, 2, ChangeKeyReason.SkillEffect, ChangeKeyType.Remove);
    }

    //玄炁+15
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 15, BattleSource.Skill);
    }
}