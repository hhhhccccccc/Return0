using System.Collections.Generic;
using Zenject;

public class Skill1040 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4300132 - ClearAbnormalBuffAndAddGainBuff
        DoConvertBuffAbnormalToGain(Subject, 3, 200005);
    }

}