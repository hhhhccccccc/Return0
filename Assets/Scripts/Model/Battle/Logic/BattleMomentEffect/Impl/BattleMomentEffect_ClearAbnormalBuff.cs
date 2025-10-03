using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ClearAbnormalBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var removeCount = Config.ParamList[1].ToInt();
                var badBuffList = target.GetRandomBuffByType(BuffType.Abnormal, removeCount);
                foreach (var badBuff in badBuffList)
                {
                    target.ClearBuff(badBuff.BuffID);
                }
            }
        }
    }
}