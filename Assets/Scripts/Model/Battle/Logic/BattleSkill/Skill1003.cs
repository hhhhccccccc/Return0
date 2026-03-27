using System.Collections.Generic;
using System.Linq;
using Zenject;

public class Skill1003 : BattleSkillBase
{
    // Moment: 1003003 → 无条件 → 相同键数量获得对应数量随机键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                var mySkill = GetSkill();
                var otherSkill = otherUnit.GetSkill();
                if (mySkill != null && otherSkill != null)
                {
                    var myKeys = mySkill.GetKeyCostList;
                    var otherKeys = otherSkill.GetKeyCostList;
                    var sameKeys = myKeys.Intersect(otherKeys).ToList();
                    if (sameKeys.Count > 0)
                    {
                        // 效果: 相同键数量获得对应数量随机键
                        DoAddRandomKey(Subject, sameKeys.Count, ChangeKeyReason.SkillEffect);
                    }
                }
            }
        }
    }
}