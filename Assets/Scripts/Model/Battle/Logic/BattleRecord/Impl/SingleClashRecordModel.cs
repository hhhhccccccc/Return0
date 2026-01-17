using System;
using System.Collections.Generic;
using cfg;

public class SingleClashRecordModel : BattleRecordModel
{
    public override BattleClashType BattleClashType => BattleClashType.SingleClash;
    
    public bool CheckSelfCostInClash { get; set; }
    public bool CheckOtherCostInClash { get; set; }

    
    
    public override void Recycle()
    {
        base.Recycle();
        CheckSelfCostInClash = false;
        CheckOtherCostInClash = false;
    }
}
