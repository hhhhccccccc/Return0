using System.Collections.Generic;
using Zenject;

public class Skill4076 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900013 - ChangeActionWheel
        Subject.ChangeActionWheel(-3);
    }

}