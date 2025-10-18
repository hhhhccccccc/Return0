using System;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect4019002 : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var keyCount = target.GetAllKeyCount();
                var addXuanQi = keyCount * Config.ParamList[1];
                var addHp = keyCount * (Config.ParamList[2] + Config.ParamList[3] * target.Gr);
                target.ChangeProperty(BattlePropertyType.XuanQi, addXuanQi);
                target.ChangeProperty(BattlePropertyType.Hp, addHp);
                if (keyCount >= Config.ParamList[4].ToInt())
                {
                    var badBuffList = target.GetRandomBuffByType(BuffType.Abnormal, Config.ParamList[5].ToInt());
                    foreach (var badBuff in badBuffList)
                    {
                        target.ClearBuff(badBuff.BuffID);
                    }
                }
            }
        }
    }
}