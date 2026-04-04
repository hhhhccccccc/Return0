using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4066 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 11;
    }

    //获得3层力增状态3层技增状态，获得1次行动次数
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddActionTimes(Subject, 1);
    }
    
    //玄炁+5
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 5, BattleSource.Skill);
    }
}