using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill4019 : BattleSkillBase
{
    //本次行动延迟3息
    
    //消耗全部的键，平均每消耗一个键玄炁+5并恢复x+GR*y体，若消耗的键大于5还会减少1层负面状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var keyCount = Subject.GetAllKeyCount();
            var addXuanQi = keyCount * 5f;
            var addHp = keyCount * (6 + 0.6f * Subject.Gr);
            DoChangeProperty(Subject, BattlePropertyType.XuanQi, addXuanQi, BattleSource.Skill);
            DoHealHp(Subject, addHp, BattleSource.Skill);
            if (keyCount > 5)
            {
                var badBuffList = Subject.GetRandomBuffByType(BuffType.Abnormal, 1);
                foreach (var badBuff in badBuffList)
                {
                    DoClearBuff(Subject, badBuff.BuffID);
                }
            }

            DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
        }
    }
}