using System.Collections.Generic;
using Zenject;

public class Skill3026 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 111014103 - AddBuff
        DoAddBuff(Subject, 10141, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}