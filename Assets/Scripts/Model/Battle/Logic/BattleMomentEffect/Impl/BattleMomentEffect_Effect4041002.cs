using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect4041002 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                BattleBuffManager.AddBuff(target, 30351, Subject, 2, null, MomentType);
            }
        }
    }
}