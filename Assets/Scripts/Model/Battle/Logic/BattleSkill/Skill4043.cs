using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4043 : BattleSkillBase
{
    //施加5层技衰状态和1层失衡状态，若与杀式交锋则敌手因招式效果获得的炁-100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffJiShuai, Subject, 5, null, BattleMomentType.BeforeClash);
        DoAddBuff(clashUnit, GameConst.Battle.BuffShiHeng, Subject, 1, null, BattleMomentType.BeforeClash);
        DoAddBuff(clashUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
    }

    //对目标施加5层技衰状态和1层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffJiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction); 
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}