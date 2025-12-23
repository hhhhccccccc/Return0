using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2026: BattleSkillBase
{
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        var keyCount = Subject.GetAllKeyCount();
        var need = Config.ParamEx[0].ToInt();
        if (keyCount >= need)
        {
            Subject.RemoveRandomKey(need, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
            return new BattleSkillRepeatData
            {
                SkillID = SkillID,
                VariantID = VariantID,
                TargetID = Target.EntityID,
                MaxRepeatCount = 999999999,
                IfLostChangeToOther = false
            };
        }

        return null;
    }
}