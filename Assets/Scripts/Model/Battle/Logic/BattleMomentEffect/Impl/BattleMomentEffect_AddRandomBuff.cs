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
        var spellCasterList = GetUnitByParamID(Config.ParamList[0]);
        var targetList = GetUnitByParamID(Config.ParamList[1]);
        if (spellCasterList.Count > 0 && targetList.Count > 0)
        {
            var buffData = ConfigHelper.RandomCommonPool(Config.ParamList[2].ToInt());
            foreach (var data in buffData)
            {
                BattleBuffManager.AddBuff(targetList[0], data.ID, spellCasterList[0], data.Num, null, MomentType);
            }
        }
    }
}