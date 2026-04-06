using System.Collections.Generic;
using cfg;

public class BattleBuff40161 : BattleBuffPotion
{
    private List<BattleKeyType> List = new()
    {
        BattleKeyType.KeyLeft
    };
    protected override void OnRoundEnd()
    {
        DoChangeKeyList(Subject, List, true, ChangeKeyReason.Item);
    }
}
