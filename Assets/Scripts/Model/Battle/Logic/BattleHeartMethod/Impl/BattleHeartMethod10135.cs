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
        if (buffID == GameConst.Battle.Buff20341 && layerCount > 0)
        {
            Subject.AddRandomKey(GetParamInt(0) * layerCount, ChangeKeyReason.HeartMethodEffect);
            Subject.ChangeProperty(BattlePropertyType.GangQi, GetParamFloat(1) * layerCount, BattleSource.HeartMethod);
            Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamFloat(2) * layerCount, BattleSource.HeartMethod);
        }
    }
}