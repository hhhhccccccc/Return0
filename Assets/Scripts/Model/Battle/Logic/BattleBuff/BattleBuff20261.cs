using System.Collections.Generic;
using cfg;
using System.Linq;
public class BattleBuff20261 : BattleBuffBase
{
    protected override void OnKeyReduce(List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (reason != ChangeKeyReason.SkillCost)
        {
            return;
        }

        if (changeType != ChangeKeyType.Cost)
        {
            return;
        }

        var count = changeKeyData.Count(o => o.KeyType == BattleKeyType.KeyLeft);
        DoReduceBuffLayerCount(Subject, GetConfigParamInt(0), GetConfigParamInt(2) * count);
        DoReduceBuffLayerCount(Subject, GetConfigParamInt(1), GetConfigParamInt(2) * count);
    }
}
