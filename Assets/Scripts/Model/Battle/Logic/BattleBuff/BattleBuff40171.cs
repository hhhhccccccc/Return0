using System.Collections.Generic;
using cfg;

public class BattleBuff40171 : BattleBuffPotion
{
    private List<BattleKeyType> List = new()
    {
        BattleKeyType.KeyRight
    };
    protected override void OnRoundEnd()
    {
        Subject.ChangeKeyList(List, true, ChangeKeyReason.Item);
    }
}
