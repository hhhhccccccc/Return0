using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1044 : BattleSkillBase
{
    //随机获得5个键
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }

    //将持有键替换为不同的键各2个
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
        DoAddAllKey(Subject, 2, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
    }

    //玄炁+55
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 55, BattleSource.Skill);
    }
}