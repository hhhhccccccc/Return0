using System;
using System.Collections.Generic;
using cfg;

public class BattleMomentEffect_RemoveAllKeyAndAddAllKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            subject.RemoveAllKey();
            var count = Config.ParamList[1].ToInt();
            subject.ChangeKey(BattleKeyType.KeyUp, count);
            subject.ChangeKey(BattleKeyType.KeyDown, count);
            subject.ChangeKey(BattleKeyType.KeyLeft, count);
            subject.ChangeKey(BattleKeyType.KeyRight, count);
        }
    }
}