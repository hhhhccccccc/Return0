using System.Collections.Generic;
using Zenject;

public class Skill4020 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

}