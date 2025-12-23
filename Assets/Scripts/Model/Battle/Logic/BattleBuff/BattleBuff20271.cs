using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20271 : BattleBuffBase
{
    protected override void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (reason == ChangeKeyReason.SkillCost && keyType == BattleKeyType.KeyRight && changeType == ChangeKeyType.Cost)
        {
            var count = Math.Abs(changeKeyData.Count);
            Subject.ReduceBuffLayerCount(Config.ParamEx[0].ToInt(), Config.ParamEx[2].ToInt() * count);
            Subject.ReduceBuffLayerCount(Config.ParamEx[1].ToInt(), Config.ParamEx[2].ToInt() * count);
        }
    }
}
