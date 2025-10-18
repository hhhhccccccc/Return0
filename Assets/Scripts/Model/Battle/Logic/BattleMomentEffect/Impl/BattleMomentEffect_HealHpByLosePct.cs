using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_HealHpByLosePct : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var pct = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                var hp = target.GetProperty(BattlePropertyType.Hp);
                var maxHp = target.GetProperty(BattlePropertyType.MaxHp);
                var delta = maxHp - hp;
                if (delta > 0)
                {
                    var heal = delta * pct;
                    target.ChangeProperty(BattlePropertyType.Hp, heal);
                }
            }
        }
    }
}