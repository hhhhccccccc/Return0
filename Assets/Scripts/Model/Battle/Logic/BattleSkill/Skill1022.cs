using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1022 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        if (CheckLastUseSkillIsBeCounter(Subject, true))
        {
            return 1;
        }
        return 0;
    }
    
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

    //玄炁+75%，若被破招则下次使用该招式不会被破招
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.75f, BattleSource.Skill);
    }
}