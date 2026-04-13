using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3076 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (Subject.ActionWheel < Target.ActionWheel)
        {
            var delta = Target.ActionWheel - Subject.ActionWheel;
            if (Subject.HasBuff(GameConst.Battle.BuffQinHuaShen))
            {
                DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, delta, null, BattleMomentType.ReleaseSkillAction);
            }
            else//没buff添加一半
            {
                DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, (int)(Math.Ceiling(delta / 2.0f)), null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }

    //玄炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}