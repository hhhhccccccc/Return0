using System;
using System.Collections.Generic;
using cfg;

public class BattleMomentEffect_ChangeSomeKey : BattleMomentEffect
{
    private List<int> AddKeyList = new();
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var count = Config.ParamList[1].ToInt();
                count = Math.Min(target.GetKeyCount(), count);
                var hasKeyList = target.GetKeyList().Clone();
                AddKeyList.Clear();
                for (var i = 1; i <= count; i++)
                {
                    var removeKey = Util.GetRandom(hasKeyList);
                    hasKeyList.Remove(removeKey);
                    target.ChangeKey((BattleKeyType)removeKey, -1);
                    AddKeyList.Add((int)(Util.GetRandomKey(1, removeKey)[0]));
                }

                foreach (var key in AddKeyList)
                {
                    target.ChangeKey((BattleKeyType)key, 1);
                }
            }
        }
    }
}