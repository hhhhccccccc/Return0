using System;
using System.Collections.Generic;
using cfg;

public class BattleMomentEffect_ChangeSomeKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var count = Config.ParamList[1].ToInt();
            count = Math.Min(subject.GetKeyCount(), count);
            var hasKeyList = subject.GetKeyList().Clone();
            for (var i = 1; i <= count; i++)
            {
                var removeKey = Util.GetRandom(hasKeyList);
                hasKeyList.Remove(removeKey);
            }

            var addKeyList = Util.GetRandomKey(count);
            foreach (var keyType in addKeyList)
            {
                subject.ChangeKey(keyType, 1);
            }
        }
    }
}