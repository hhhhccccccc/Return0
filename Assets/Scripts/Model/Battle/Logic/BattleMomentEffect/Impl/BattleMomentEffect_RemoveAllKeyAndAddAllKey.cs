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
            var list = new List<BattleKeyType>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(BattleKeyType.KeyUp);
                list.Add(BattleKeyType.KeyDown);
                list.Add(BattleKeyType.KeyLeft);
                list.Add(BattleKeyType.KeyRight);
            }
            foreach (var target in targetList)
            {
                target.RemoveAllKey(ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
                Subject.ChangeKeyList(list, true, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
            }
        }
    }
}