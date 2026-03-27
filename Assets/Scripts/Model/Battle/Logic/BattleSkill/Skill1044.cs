using System.Collections.Generic;
using Zenject;

public class Skill1044 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 400005 - AddRandomKey
        Subject.AddRandomKey(5, (ChangeKeyReason)4);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4100002 - RemoveAllKeyAndAddAllKey
        DoRemoveAllKeyAndAddAllKey(Subject, 2);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102009 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 55);
    }

}