using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现 消耗键后消耗全部的键然后补充到上限
public class BattleHeartMethod10106 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(2);
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
            if (Accumulate >= GetConfigParamInt(1))
            {
                Accumulate -= GetConfigParamInt(1);
                Subject.AddActionTimes(Times);
                EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
            }
        }
        else
        {
            if (changeType == ChangeKeyType.Cost)
            {
                if (Subject.GetAllKeyCount() > 0)
                {
                    var keyList = Subject.GetAllKeyTypeList().Select(o => (BattleKeyType)o).ToList();
                    Subject.ChangeKeyList(keyList, false, ChangeKeyReason.HeartMethodEffect, ChangeKeyType.Cost);
                }

                var max = Subject.GetKeyPropertyMax();
                var curr = Subject.GetAllKeyCount();
                if (curr != max)
                {
                    Subject.AddRandomKey(max, ChangeKeyReason.HeartMethodEffect);
                }
            }
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        Accumulate = 0;
    }
}