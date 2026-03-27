using System.Collections.Generic;
using Zenject;

public class Skill3018 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

}