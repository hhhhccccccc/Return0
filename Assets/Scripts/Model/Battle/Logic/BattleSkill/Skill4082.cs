using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4082 : BattleSkillBase
{
    //清除目标全部增益状态并施加4层昏沉状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuffByType(Target, BuffType.Gain, 4);
        DoAddBuff(Target, GameConst.Battle.BuffHunChen, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }
}