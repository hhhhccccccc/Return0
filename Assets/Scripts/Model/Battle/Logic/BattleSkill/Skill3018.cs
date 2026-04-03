using System.Collections.Generic;
using Zenject;

public class Skill3018 : BattleSkillBase
{
    //获得1次行动次数
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
    }
}