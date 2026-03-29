using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1036 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //清除自身3个异常状态，若清除数量不超过3个则每少1个给予1层武增1层力增1层巧增
        DoClearAbnormalBuffAndAddGainBuff(Subject, 200004, 3, BattleMomentType.ReleaseSkillAction);
    }

}