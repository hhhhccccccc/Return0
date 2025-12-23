using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill4019 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var keyCount = Subject.GetAllKeyCount();
            var addXuanQi = keyCount * Config.ParamEx[0];
            var addHp = keyCount * (Config.ParamEx[1] + Config.ParamEx[2] * Subject.Gr);
            Subject.ChangeProperty(BattlePropertyType.XuanQi, addXuanQi);
            Subject.HealHp(addHp, BattleSource.Skill);
            if (keyCount >= Config.ParamEx[3].ToInt())
            {
                var badBuffList = Subject.GetRandomBuffByType(BuffType.Abnormal, Config.ParamEx[4].ToInt());
                foreach (var badBuff in badBuffList)
                {
                    Subject.ClearBuff(badBuff.BuffID);
                }
            }
        }
    }
}