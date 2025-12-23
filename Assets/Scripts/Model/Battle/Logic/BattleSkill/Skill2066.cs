using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill2066 : BattleSkillBase
{
    private const int BuffID = 20081;
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var skill = target.GetSkill();
            var costKeyCount = skill.GetKeyCostList.Count;
            BattleBuffManager.AddBuff(target, BuffID, Subject, costKeyCount, null, BattleMomentType.BeforeClash);
        }
    }
}