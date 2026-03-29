using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1009 : BattleSkillBase
{
    //刚炁+15，玄炁+15
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 15, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 15, BattleSource.Skill);
    }
}