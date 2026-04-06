using System;
using System.Collections.Generic;
using cfg;
using Zenject;
using System.Linq;

public class BattleBuff20221 : BattleBuffBase
{
    /// <summary>
    /// //todo 行动开始在本次行动中随机污染x个键，使用招式每消耗1个被污染的键获得1层妖毒侵蚀状态（正常的键优先消耗），因任何原因减少被污染的键时等量减少妖毒状态的层数
    /// </summary>
    
    
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

    protected override void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (IsTrigger)
        {
            if (changeType != ChangeKeyType.Cost)
            {
                return;
            }
            
            //使用招式每消耗1个被污染的键获得1层妖毒侵蚀状态
            if (reason == ChangeKeyReason.SkillCost)
            {
                foreach (var data in changeKeyData)
                {
                    if (TriggerKeyDataList.Contains(data.KeyGuid))
                    {
                        DoAddBuff(Subject, GameConst.Battle.BuffYaoDuQinShi, Subject, 1, null, BattleMomentType.None);
                    }
                }
            }

            var count = changeKeyData.Count(o => TriggerKeyDataList.Contains(o.KeyGuid));
            DoReduceBuffLayerCount(Subject, BuffID, count);
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
    protected override void OnBuffRecycle()
    {
        IsTrigger = false;
        TriggerKeyDataList.Clear();
    }
}
