using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4063 : BattleSkillBase
{
    //施加3层毒瘴状态和5层赤沸状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffDuZhang, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffChiFei, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //双方获得1层毒瘴状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, 1, null, BattleMomentType.BeforeClash);
        var clasUnit = GetOtherUnit(paramModel);
        DoAddBuff(clasUnit, GameConst.Battle.BuffDuZhang, Subject, 1, null, BattleMomentType.BeforeClash);
    }
}