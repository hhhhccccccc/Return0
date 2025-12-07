using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10134 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Register<UnitDieEventModel>(OnUnitDie);
    }

    private void OnUnitDie(UnitDieEventModel model)
    {
        var target = BattleManager.GetUnit(model.DieID);
        var count = target.GetBuffCountByID(GameConst.Battle.Buff20341);
        var heal = (GetParamFloat(0) + GetParamFloat(1) * Subject.Gr) * (1 + count * GetParamFloat(2));
    }
}