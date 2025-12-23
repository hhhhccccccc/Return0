using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill2008 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var targetSkill = target.GetSkill();
            if (targetSkill != null && targetSkill.Target == Subject && Target == target)
            {
                var buffID = Config.ParamEx[0].ToInt();
                var propertyID = Config.ParamEx[1].ToInt();
                var pct = Config.ParamEx[2];
                var propertyValue = target.GetProperty((BattlePropertyType)propertyID);
                propertyValue *= pct;
                BattleBuffManager.AddBuff(Subject, buffID, Subject, 1, new List<float> { propertyValue }, BattleMomentType.BeforeClash);
                BattleBuffManager.AddBuff(target, buffID, Subject, 1, new List<float> { -propertyValue }, BattleMomentType.BeforeClash);
            }
        }
    }
} 