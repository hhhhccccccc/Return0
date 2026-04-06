using System.Collections.Generic;
using cfg;

public class BattleHeartMethod10104 : BattleHeartMethodBase
{
    public override void KeyReduce(List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (Subject.GetAllKeyCount() <= 0)
        {
            DoAddRandomKey(Subject, GetConfigParamInt(0), ChangeKeyReason.HeartMethodEffect);
        }
    }
}