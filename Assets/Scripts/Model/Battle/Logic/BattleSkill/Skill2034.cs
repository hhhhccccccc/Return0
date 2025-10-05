using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill2034 : BattleSkillBase
{
    public override void ActionWheelStart()
    {
        base.ActionWheelStart();
        var hasKey = Subject.GetKeyList();
        var upCount = hasKey.Count(key => key == (int)BattleKeyType.KeyUp);
        var downCount = hasKey.Count(key => key == (int)BattleKeyType.KeyDown);
        var leftCount = hasKey.Count(key => key == (int)BattleKeyType.KeyLeft);
        var rightCount = hasKey.Count(key => key == (int)BattleKeyType.KeyRight);
        var removeTwoList1 = new List<int>();
        var removeTwoList2 = new List<int>();
        
        if (upCount >= 2)
        {
            removeTwoList1.Add(1);
            removeTwoList2.Add(1);
        }
        if (downCount >= 2)
        {
            removeTwoList1.Add(2);
            removeTwoList2.Add(2);
        }
        if (leftCount >= 2)
        {
            removeTwoList1.Add(3);
            removeTwoList2.Add(3);
        }
        if (rightCount >= 2)
        {
            removeTwoList1.Add(4);
            removeTwoList2.Add(4);
        }

        var removeTwoKeyType1 = Util.GetRandom(removeTwoList1);
        Subject.ChangeKey((BattleKeyType)removeTwoKeyType1, -2);
        if (removeTwoList2.Contains(removeTwoKeyType1))
        {
            removeTwoList2.Remove(removeTwoKeyType1);
        }

        if (removeTwoList2.Count > 0)
        {
            var removeOneKeyType = Util.GetRandom(removeTwoList2);
            Subject.ChangeKey((BattleKeyType)removeOneKeyType, -2);
        }
    }
}