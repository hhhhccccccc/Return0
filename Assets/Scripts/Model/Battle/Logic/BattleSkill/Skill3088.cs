using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3088: BattleSkillBase
{
    //获得1层武增状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //在下一息重复该行动，至多重复2次
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        return new BattleSkillRepeatData
        {
            SkillID = SkillID,
            VariantID = VariantID,
            TargetID = Target.EntityID,
            MaxRepeatCount = 2,
            IfLostChangeToOther = false
        };
    }
}