using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_AddRandomBuff : BattleMomentEffect
{
    [Inject] private ConfigHelper ConfigHelper { get; set; }
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var spellCaster = GetUnitByParamID(Config.ParamList[0]);
        var target = GetUnitByParamID(Config.ParamList[1]);
        if (spellCaster != null && target != null)
        {
            var buffData = ConfigHelper.RandomCommonPool(Config.ParamList[2].ToInt());
            foreach (var data in buffData)
            {
                BattleBuffManager.AddBuff(target, data.ID, spellCaster, data.Num);
            }
        }
    }
}