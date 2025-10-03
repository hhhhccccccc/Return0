using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ClearAbnormalBuffAndAddGainBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private ConfigHelper ConfigHelper { get; set; }
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

                var addCount = removeCount - badBuffList.Count;
                if (addCount > 0)
                {
                    var poolID = Config.ParamList[2].ToInt();
                    var buffDataList = ConfigHelper.RandomCommonPool(poolID);
                    foreach (var buffData in buffDataList)
                    {
                        BattleBuffManager.AddBuff(target, buffData.ID, target, buffData.Num * addCount);
                    }
                }
            }
        }
    }
}