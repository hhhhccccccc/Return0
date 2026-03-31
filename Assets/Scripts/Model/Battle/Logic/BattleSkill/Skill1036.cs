using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1036 : BattleSkillBase
{
    //清除自身3个异常状态，若清除数量不超过3个则每少1个给予1层武增1层力增1层巧增
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearAbnormalBuffAndAddGainBuff(Subject, 200004, 3, BattleMomentType.ReleaseSkillAction);
    }
}