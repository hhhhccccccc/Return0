using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10135 : BattleHeartMethodBase
{
    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (buffID == GameConst.Battle.BuffDuZhang && layerCount > 0)
        {
            var addKeyList = Subject.AddRandomKey(GetParamInt(0) * layerCount, ChangeKeyReason.HeartMethodEffect);
            var finalGangQi = Subject.ChangeProperty(BattlePropertyType.GangQi, GetParamFloat(1) * layerCount, BattleSource.HeartMethod);
            var finalXuanQi = Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamFloat(2) * layerCount, BattleSource.HeartMethod);
            var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.HeartMethod10135, finalGangQi, finalXuanQi);
            if (addKeyList is { Count: > 0 })
            {
                viewModel.AddKeyList(addKeyList);
            }
            EnqueueViewModel(viewModel);
        }
    }
}