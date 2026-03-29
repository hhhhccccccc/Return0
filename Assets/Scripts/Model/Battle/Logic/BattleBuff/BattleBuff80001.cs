using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff80001 : BattleBuffBase
{
    public override void RoundEnd()
    {
        if (CanTriggerBuffEffect())
        {
            OnRoundEnd();
        }

        if (Subject.BattleMomentManager.GetTreasureByFeature(TreasureFeature.JinGangSan) != null)
        {
            if (Subject.RoundBeDirectDamageTimes >= 1)
            {
                return;
            }
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundEnd);
    }
}
