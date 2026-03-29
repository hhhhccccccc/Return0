using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1044 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 400005 - AddRandomKey
        Subject.AddRandomKey(5, ChangeKeyReason.SkillEffect);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //将持有键替换为不同的键各2个
        DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
        DoAddAllKey(Subject, 2, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102008 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 55);
    }

}