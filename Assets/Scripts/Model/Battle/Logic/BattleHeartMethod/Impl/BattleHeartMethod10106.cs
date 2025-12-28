using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10106 : BattleHeartMethodBase
{
    private int Accumulate { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Accumulate = 0;
    }

    public override void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (isAdd)
        {
            Accumulate += changeKeyData.Count;
            if (Accumulate >= GetParamInt(1))
            {
                Accumulate -= GetParamInt(1);
                Subject.AddActionTimes(GetParamInt(2));
            }
        }
        else
        {
            if (changeType == ChangeKeyType.Cost)
            {
                if (Subject.GetAllKeyCount() > 0)
                {
                    var keyList = Subject.GetAllKeyTypeList();
                    Subject.ChangeKeyList(keyList, false, ChangeKeyReason.HeartMethodEffect, ChangeKeyType.Cost);
                }

                var max = Subject.GetKeyPropertyMax();
                var curr = Subject.GetAllKeyCount();
                if (curr != max)
                {
                    Subject.AddRandomKey(max, ChangeKeyReason.HeartMethodEffect, ChangeKeyType.None);
                }
            }
        }
    }

    protected override void OnRecycle()
    {
        Accumulate = 0;
    }
}