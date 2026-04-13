using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3052 : BattleSkillBase
{
    //本次行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 1);
    }

    //未产生交锋则施加1层失衡
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel { BattleClashType: BattleClashType.SingleAction })
        {
             DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }

    //玄炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}