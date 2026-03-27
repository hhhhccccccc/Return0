using System.Collections.Generic;
using Zenject;

public class Skill2071 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 111004101 - AddBuff
        DoAddBuff(Subject, 10041, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}