using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10109 : BattleHeartMethodBase
{
    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (isReduceHpMax)
        {
            return;
        }

        if (damageType != DamageType.Direct)
        {
            return;
        }
        
        if (Subject.RoundBeDirectDamageTimes == 1)
        {
            var addKeyList = Subject.AddRandomKey(GetConfigParamInt(0), ChangeKeyReason.HeartMethodEffect);
            if (addKeyList is { Count: > 0 })
            {
                var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey);
                viewModel.AddKeyList(addKeyList);
                EnqueueViewModel(viewModel);
            }
        }
    }
}