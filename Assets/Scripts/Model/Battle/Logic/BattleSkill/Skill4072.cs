using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4072 : BattleSkillBase
{
    //玄炁+30，获得2层借法
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 30, BattleSource.Skill);
        DoAddBuff(Subject, GameConst.Battle.BuffJieFa, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //刚炁与玄炁取平均值更变
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        var gangQi = Subject.GetProperty(BattlePropertyType.GangQi);
        var xuanQi = Subject.GetProperty(BattlePropertyType.XuanQi);
        var average = (gangQi + xuanQi) / 2;
        DoSetProperty(Subject, BattlePropertyType.GangQi, average, BattleSource.Skill);
        DoSetProperty(Subject, BattlePropertyType.XuanQi, average, BattleSource.Skill);
    }
}