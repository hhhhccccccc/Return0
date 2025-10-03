using System;
using System.Collections.Generic;
using cfg;

public class BattleMomentEffect_RemoveAllKeyAndAddAllKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var count = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                target.RemoveAllKey();
                target.ChangeKey(BattleKeyType.KeyUp, count);
                target.ChangeKey(BattleKeyType.KeyDown, count);
                target.ChangeKey(BattleKeyType.KeyLeft, count);
                target.ChangeKey(BattleKeyType.KeyRight, count);
            }
        }
    }
}