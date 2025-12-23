using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10100 : BattleHeartMethodBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.BattleClashType == BattleClashType.SingleAction)
            {
                var target = BattleManager.GetUnit(model.OtherID);
                if (!target.HasBuff(GameConst.Battle.Buff20111))
                {
                    BattleBuffManager.AddBuff(target, GameConst.Battle.Buff20111, Subject, GetParamInt(0));
                }
            }
        }
    }
}