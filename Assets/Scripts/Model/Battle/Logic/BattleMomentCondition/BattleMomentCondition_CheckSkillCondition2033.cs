using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentCondition_CheckSkillCondition2033 : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(1);
        if (target != null)
        {
            var hasKey = Subject.GetAllKeyTypeList();
            var upCount = hasKey.Count(key => key == (int)BattleKeyType.KeyUp);
            var downCount = hasKey.Count(key => key == (int)BattleKeyType.KeyDown);
            var leftCount = hasKey.Count(key => key == (int)BattleKeyType.KeyLeft);
            var rightCount = hasKey.Count(key => key == (int)BattleKeyType.KeyRight);

            if (hasKey.Distinct().Count() >= 2 && (upCount >= 2 || downCount >= 2 || leftCount >= 2 || rightCount >= 2))
            {
                return true;
            }
        }
        
        return false;
    }
}