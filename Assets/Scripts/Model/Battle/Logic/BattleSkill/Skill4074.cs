using System.Collections.Generic;
using Zenject;

public class Skill4074 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3800005 - GetShieldBuffByTechPct
        // TODO: GetShieldBuffByTechPct target=5
        // 效果: 152001101 - AddBuff
        // TODO: AddBuff [caster=1, target=5]
    }

}