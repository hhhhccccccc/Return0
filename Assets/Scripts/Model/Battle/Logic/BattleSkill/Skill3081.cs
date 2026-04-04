using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3081 : BattleSkillBase
{
    //未产生交锋则施加1层法式禁状态和2层术衰状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel { BattleClashType: BattleClashType.SingleAction })
        {
            if (Target != null) DoAddBuff(Target, GameConst.Battle.BuffFaShiJin, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
            if (Target != null) DoAddBuff(Target, GameConst.Battle.BuffShuShuai, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}