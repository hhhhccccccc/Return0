using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill4014 : BattleSkillBase
{
    //获得1层傍剑（标记目标）
    public override void DoDesitionAction(bool isPreDesition)
    {
        var buff = DoAddBuff(Subject, GameConst.Battle.BuffBangJian, Subject, 1, null, BattleMomentType.DoDesitionAction);
        if (buff != null)
        {
            buff.SetTarget(Target);
        }
    }
}