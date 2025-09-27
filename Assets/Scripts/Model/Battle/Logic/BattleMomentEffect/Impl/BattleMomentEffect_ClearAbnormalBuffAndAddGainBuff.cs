using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ClearAbnormalBuffAndAddGainBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    private List<int> AddBuffList = new()
    {
        10071,
        10091,
        10111,
    };
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var removeCount = Config.ParamList[1].ToInt();
            var badBuffList = target.GetRandomBuffByType(BuffType.Abnormal, removeCount);
            foreach (var badBuff in badBuffList)
            {
                target.ClearBuff(badBuff.BuffID);
            }

            var delta = removeCount - badBuffList.Count;
            delta *= Config.ParamList[2].ToInt();
            if (delta > 0)
            {
                foreach (var addBuffID in AddBuffList)
                {
                    BattleBuffManager.AddBuff(target, addBuffID, target, delta);
                }
            }
        }
    }
}