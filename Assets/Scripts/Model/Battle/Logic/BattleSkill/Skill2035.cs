using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2035 : BattleSkillBase
{
    //施加3层技衰状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffJiShuai, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //玄炁+15
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 15, BattleSource.Skill);
    }
}