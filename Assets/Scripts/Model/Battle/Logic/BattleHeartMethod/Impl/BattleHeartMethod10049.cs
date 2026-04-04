using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10049 : BattleHeartMethodBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var skillA = BattleManager.GetUnit(model.GetSelfID(Subject.EntityID)).GetSkill();
            var skillB = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID)).GetSkill();
            if (skillA != null && skillB != null)
            {
                var listA = skillA.GetKeyCostList;
                var listB = skillB.GetKeyCostList;
                var min = Math.Min(listA.Count, listB.Count);
                var count = 0;
                for (int i = 0; i < min; i++)
                {
                    var a = (BattleKeyType)listA[i];
                    var b = (BattleKeyType)listB[i];
                    if ((a == BattleKeyType.KeyUp && b == BattleKeyType.KeyDown)
                        || (a == BattleKeyType.KeyDown && b == BattleKeyType.KeyUp)
                        || (a == BattleKeyType.KeyLeft && b == BattleKeyType.KeyRight)
                        || (a == BattleKeyType.KeyRight && b == BattleKeyType.KeyLeft))
                    {
                        count++;
                    }
                }

                if (count >= 2)
                {
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffJiaoMing, Subject, GetConfigParamInt(0));
                }
            }
        }
    }
}