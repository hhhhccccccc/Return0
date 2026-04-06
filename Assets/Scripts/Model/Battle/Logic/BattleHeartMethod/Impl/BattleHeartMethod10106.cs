using System.Collections.Generic;
using System.Linq;
using cfg;

public class BattleHeartMethod10106 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(2);
    private int Accumulate { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Accumulate = 0;
    }

    public override void AfterChangeKey(List<BattleKey> changeKeyList, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (isAdd)
        {
            Accumulate += changeKeyList.Count;
            if (Accumulate >= GetConfigParamInt(1))
            {
                Accumulate -= GetConfigParamInt(1);
                DoAddActionTimes(Subject, Times);
            }
        }
        else
        {
            if (changeType == ChangeKeyType.Cost)
            {
                if (Subject.GetAllKeyCount() > 0)
                {
                    var keyList = Subject.GetAllKeyTypeList().Select(o => (BattleKeyType)o).ToList();
                    DoChangeKeyList(Subject, keyList, false, ChangeKeyReason.HeartMethodEffect, ChangeKeyType.Cost);
                }

                var max = Subject.GetKeyPropertyMax();
                var curr = Subject.GetAllKeyCount();
                if (curr != max)
                {
                    DoAddRandomKey(Subject, max, ChangeKeyReason.HeartMethodEffect);
                }
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        Accumulate = 0;
    }
}