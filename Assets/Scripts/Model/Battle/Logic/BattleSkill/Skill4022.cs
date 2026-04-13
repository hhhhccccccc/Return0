using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4022 : BattleSkillBase
{
    //本次行动不会被未带有→类留劲状态的敌手破招
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 5;
    }
    
    
    //获得5层力增状态5层技增状态，获得1次行动次数
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddActionTimes(Subject, 1);
    }

    //玄炁+9500
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 9500, BattleSource.Skill);
    }
}