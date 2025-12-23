using System.Collections.Generic;
using cfg;

public class BattleBuff40141 : BattleBuffPotion
{
    private List<BattleKeyType> List = new()
    {
        BattleKeyType.KeyUp
    };
    protected override void OnRoundEnd()
    {
        Subject.ChangeKeyList(List, true, ChangeKeyReason.Item);
    }
}
