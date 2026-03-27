using System.Collections.Generic;
using Zenject;

public class Skill4051 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900001 - ChangeActionWheel
        Subject.ChangeActionWheel(1);
    }

}