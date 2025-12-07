using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20221 : BattleBuffBase
{
    private bool IsTrigger { get; set; }
    private List<int> TriggerKeyDataList = new();
    protected override void OnBeforeDoDesitionAction()
    {
        IsTrigger = true;
        var dataList = Subject.PollutionRandomKey(LayerCount);
        if (dataList != null)
        {
            foreach (var data in dataList)
            {
                TriggerKeyDataList.Add(data.KeyGuid);   
            }
        }
    }

    protected override void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        if (IsTrigger)
        {
            //使用招式每消耗1个被污染的键获得1层妖毒侵蚀状态
            if (reason == ChangeKeyReason.SkillCost)
            {
                foreach (var data in changeKeyData)
                {
                    if (TriggerKeyDataList.Contains(data.KeyGuid))
                    {
                        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff20231, Subject, 1);
                    }
                }
            }
            
            foreach (var data in changeKeyData)
            {
                if (TriggerKeyDataList.Contains(data.KeyGuid))
                {
                    ReduceLayerCount(1);
                    if (!Valid)
                    {
                        return;
                    }
                }
            }
        }
    }

    protected override void OnSkillEnd(BattleSkillBase skill)
    {
        IsTrigger = false;
        foreach (var data in TriggerKeyDataList)
        {
            Subject.UnPollutionKey(data);
        }
        base.OnSkillEnd(skill);
    }

    public override void Recycle()
    {
        IsTrigger = false;
        TriggerKeyDataList.Clear();
        base.Recycle();
    }
}
