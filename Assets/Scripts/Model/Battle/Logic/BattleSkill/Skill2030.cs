using System.Collections.Generic;
using Zenject;

public class Skill2030 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 400003 - AddRandomKey
        Subject.AddRandomKey(3, (ChangeKeyReason)4);
    }

}