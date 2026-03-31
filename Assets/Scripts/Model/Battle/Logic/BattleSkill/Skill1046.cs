using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1046 : BattleSkillBase
{
    //清除自身3个异常状态，若清除数量不超过3个则每少1个给予2层武增2层力增2层巧增
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearAbnormalBuffAndAddGainBuff(Subject, 200005, 3, BattleMomentType.ReleaseSkillAction);
    }
}