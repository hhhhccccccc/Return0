using System.Collections.Generic;
using Zenject;

public class Skill1036 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4300131 - ClearAbnormalBuffAndAddGainBuff
        DoConvertBuffAbnormalToGain(Subject, 3, 200004);
    }

}