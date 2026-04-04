using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3082 : BattleSkillBase
{
    //未产生交锋则施加2层技式禁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel { BattleClashType: BattleClashType.SingleAction })
        { 
            DoAddBuff(Target, GameConst.Battle.BuffJiShiJin, Subject, 2, null, BattleMomentType.ReleaseSkillAction); 
        }
    }
}