using System;
using System.Collections.Generic;
using cfg;

public class DoubleClashRecordModel : BattleRecordModel
{
    public override BattleClashType BattleClashType => BattleClashType.DoubleClash;
    
    public bool CheckSelfCostInClash { get; set; }
    public bool CheckOtherCostInClash { get; set; }
    
    public override void Recycle()
    {
        base.Recycle();
        CheckSelfCostInClash = false;
        CheckOtherCostInClash = false;
    }
}
