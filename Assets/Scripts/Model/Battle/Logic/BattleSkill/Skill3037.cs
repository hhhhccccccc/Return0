using System.Collections.Generic;
using Zenject;

public class Skill3037 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 113010101 - AddBuff
        DoAddBuff(Subject, 30101, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}