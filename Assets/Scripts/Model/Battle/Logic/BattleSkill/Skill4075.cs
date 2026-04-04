using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4075 : BattleSkillBase
{
    //玄炁增加至35
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var xuanQi = Subject.GetProperty(BattlePropertyType.XuanQi);
        if (xuanQi < 35)
        {
            DoSetProperty(Subject, BattlePropertyType.XuanQi, 35, BattleSource.Skill);
        }
    }
}