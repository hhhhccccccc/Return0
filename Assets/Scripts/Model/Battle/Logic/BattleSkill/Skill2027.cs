using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2027 : BattleSkillBase
{
    //行动延迟1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        Subject.ChangeActionWheel(-1);
    }

    //减少所有人其1个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        foreach (var unit in BattleManager.GetAllOpponentUnit(Subject.EntityID, true))
        {
            DoRemoveRandomKey(unit, 1, ChangeKeyReason.SkillEffect, ChangeKeyType.Remove);
        }
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