using System.Collections.Generic;
using Zenject;

public class Skill3018 : BattleSkillBase
{
    //获得1次行动次数
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
    }
}