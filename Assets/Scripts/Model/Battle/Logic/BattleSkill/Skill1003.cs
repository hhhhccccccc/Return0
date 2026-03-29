using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill1003 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    
    //获得对方招式构成相同的键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                var mySkill = Subject.GetSkill();
                var otherSkill = otherUnit.GetSkill();
                if (mySkill != null && otherSkill != null)
                {
                    var myKeys = mySkill.GetKeyCostList;
                    var otherKeys = otherSkill.GetKeyCostList;
                    var sameKeys = myKeys.Intersect(otherKeys).ToList();
                    if (sameKeys.Count > 0)
                    {
                        DoAddKey(Subject, sameKeys.Select(o => (BattleKeyType)o).ToList(), ChangeKeyReason.SkillEffect, ChangeKeyType.None);
                    }
                }
            }
        }
    }
}