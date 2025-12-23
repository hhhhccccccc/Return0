using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3034: BattleSkillBase
{
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        var keyCount = Subject.GetKeyCount(BattleKeyType.KeyDown);
        var need = Config.ParamEx[0].ToInt();
        if (keyCount >= need)
        {
            var list = new List<BattleKeyType>
            {
                BattleKeyType.KeyDown,
                BattleKeyType.KeyDown
            };
            Subject.ChangeKeyList(list, false, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
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