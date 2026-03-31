using System.Collections.Generic;
using Zenject;

public class Skill3011 : BattleSkillBase
{
    protected override int DontBeCounter()
    {
        return 1;
    }
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 122001103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20011, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}