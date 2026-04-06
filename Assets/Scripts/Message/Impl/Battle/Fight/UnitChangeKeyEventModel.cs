using System.Collections.Generic;
using cfg;

public class UnitChangeKeyEventModel : MessageModel
{
    public int UnitID { get; set; }
    public List<BattleKeyType> KeyTypeList = new List<BattleKeyType>();
    public ChangeKeyReason Reason { get; set; }
    public ChangeKeyType ChangeType { get; set; }
    public override void Recycle()
    {
        UnitID = 0;
        KeyTypeList.Clear();
    }
}
