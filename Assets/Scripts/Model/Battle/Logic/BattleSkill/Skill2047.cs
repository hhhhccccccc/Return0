using System.Collections.Generic;
using Zenject;

public class Skill2047 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3800004 - GetShieldBuffByTechPct
        var tech = Subject.GetProperty(BattlePropertyType.Tech); BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (tech * 0.3).ToInt(), null, BattleMomentType.ReleaseSkillAction);
    }

}