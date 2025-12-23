using System.Collections.Generic;
using cfg;

public class BattleBuff40151 : BattleBuffPotion
{
    private List<BattleKeyType> List = new()
    {
        BattleKeyType.KeyDown
    };
    protected override void OnRoundEnd()
    {
        Subject.ChangeKeyList(List, true, ChangeKeyReason.Item);
    }
}
