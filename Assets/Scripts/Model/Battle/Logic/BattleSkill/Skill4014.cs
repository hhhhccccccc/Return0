using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill4014 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        var buff = BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30091, Subject, 1);
        if (buff != null)
        {
            buff.SetTarget(Target);
        }
    }
}