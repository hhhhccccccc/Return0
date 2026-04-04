using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill4041 : BattleSkillBase
{
    //补充随机的键到达7个
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddRandomKeyToDefineCount(Subject, 7, ChangeKeyReason.SkillEffect);
    }

    //下一次术杀式的基础威力不会低于140%技（巧来方计状态）
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        BattleBuffManager.AddBuff(Subject, 30351, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}