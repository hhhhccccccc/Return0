using System;
using System.Collections.Generic;
using cfg;

public class BattleMomentEffect_ChangeSomeKey : BattleMomentEffect
{
    private List<int> AddKeyList = new();
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var count = Config.ParamList[1].ToInt();
            count = Math.Min(subject.GetKeyCount(), count);
            var hasKeyList = subject.GetKeyList().Clone();
            AddKeyList.Clear();
            for (var i = 1; i <= count; i++)
            {
                var removeKey = Util.GetRandom(hasKeyList);
                hasKeyList.Remove(removeKey);
                subject.ChangeKey((BattleKeyType)removeKey, -1);
                AddKeyList.Add((int)(Util.GetRandomKey(1, removeKey)[0]));
            }

            foreach (var key in AddKeyList)
            {
                subject.ChangeKey((BattleKeyType)key, 1);
            }
        }
    }
}