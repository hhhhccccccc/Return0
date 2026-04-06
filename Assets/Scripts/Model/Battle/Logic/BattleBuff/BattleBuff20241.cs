using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20241 : BattleBuffBase
{
    protected override void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (reason == ChangeKeyReason.SkillCost && keyType == BattleKeyType.KeyUp && changeType == ChangeKeyType.Cost)
        {
            var count = Math.Abs(changeKeyData.Count);
            DoReduceBuffLayerCount(Subject, GetConfigParamInt(0), GetConfigParamInt(2) * count);
            DoReduceBuffLayerCount(Subject, GetConfigParamInt(1), GetConfigParamInt(2) * count);
        }
    }
}
