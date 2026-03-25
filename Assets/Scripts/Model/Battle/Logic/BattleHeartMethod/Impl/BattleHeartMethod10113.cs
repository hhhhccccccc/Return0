using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10113 : BattleHeartMethodBase
{
    private float GangQi => GetParamFloat(0);
    private float XuanQi => GetParamFloat(1);
    private int KeyCount => GetParamInt(2);
    public override void EveryActionWheelStart()
    {
        var finalGangQi = Subject.ChangeProperty(BattlePropertyType.GangQi, GangQi, BattleSource.HeartMethod);
        var finalXuanQi = Subject.ChangeProperty(BattlePropertyType.XuanQi, XuanQi, BattleSource.HeartMethod);
        var addKeyList = Subject.AddRandomKey(KeyCount, ChangeKeyReason.HeartMethodEffect);
        
        var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.BattleHeartMethod10113, finalGangQi, finalXuanQi);
        if (addKeyList != null && addKeyList.Count > 0)
        {
            viewModel.AddKeyList(addKeyList);
        }
        EnqueueViewModel(viewModel);
    }
}