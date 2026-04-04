using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4067 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 11;
    }

    //获得5层力增状态5层技增状态，获得1次行动次数
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddActionTimes(Subject, 1);
    }
    
    //玄炁+5
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 5, BattleSource.Skill);
    }
}