using System;
using System.Collections.Generic;
using cfg;

public class BattleBuff20081 : BattleBuffBase
{
    /// <summary>
    /// //todo 每层使扣除键时同时扣除{[int]}的体
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    protected override void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (changeType != ChangeKeyType.Cost)
        {
            return;
        }
        
        if (keyType == BattleKeyType.KeyDown 
            || keyType == BattleKeyType.KeyUp 
            || keyType == BattleKeyType.KeyLeft
            || keyType == BattleKeyType.KeyRight)
        {
            TriggerBuffMomentByCount(Math.Abs(changeKeyData.Count), null);
        }
    }

    public override void TriggerBuffMomentByCount(int count, MomentParamModel paramModel)
    {
        if (CanTriggerBuffEffect())
        {
            var singleValue = Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
            Subject.ReduceHp(singleValue * LayerCount, DamageType.InDirect, SpellCaster.EntityID);
        }
    }
}
