using System;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect4055001 : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var value = Config.ParamList[1] + Subject.Gr * Config.ParamList[2];
            var addValue = 0.0f;
            foreach (var target in targetList)
            {
                target.ChangeProperty(BattlePropertyType.MaxHpInt, -value);
                addValue += value;
            }

            Subject.ChangeProperty(BattlePropertyType.MaxHpInt, addValue);
        }
    }
}