using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3076 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var addBuffID = Config.ParamEx[0].ToInt();
            var checkBuffID =  Config.ParamEx[1].ToInt();
            if (Subject.ActionWheel < target.ActionWheel)
            {
                var delta = target.ActionWheel - Subject.ActionWheel;
                var checkBuff = target.GetBuff(checkBuffID);
                var buffCount = checkBuff?.LayerCount ?? 0;
                if (buffCount <= 0)//没buff添加一半
                {
                    BattleBuffManager.AddBuff(target, addBuffID, Subject, (int)(Math.Ceiling(delta / 2.0f)), null, BattleMomentType.ReleaseSkillAction);
                }
                else
                {
                    BattleBuffManager.AddBuff(target, addBuffID, Subject, delta, null, BattleMomentType.ReleaseSkillAction);
                }
            }
        }
    }
}