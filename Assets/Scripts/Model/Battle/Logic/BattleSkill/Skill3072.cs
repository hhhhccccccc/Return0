using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3072 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var gangQi = target.GetProperty(BattlePropertyType.GangQi);
            var xuanQi = target.GetProperty(BattlePropertyType.XuanQi);
            if (gangQi >= xuanQi)
            {
                var cost = gangQi * Config.ParamEx[0];
                cost = Math.Min(cost, Config.ParamEx[1]);
                target.ChangeProperty(BattlePropertyType.GangQi, cost);
            }
            else
            {
                var cost = xuanQi * Config.ParamEx[0];
                cost = Math.Min(cost, Config.ParamEx[1]);
                target.ChangeProperty(BattlePropertyType.XuanQi, cost);
            }
        }
    }
}