using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3058 : BattleSkillBase
{
    //施加1层盲目状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffMangMu, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //玄炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}